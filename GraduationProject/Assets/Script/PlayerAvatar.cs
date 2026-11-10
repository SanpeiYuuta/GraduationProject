using Fusion;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

// 入力についてのデータ構造
public class InputDeta
{
    public bool pressed;
    public bool released;

    public void Reset()
    {
        pressed = false;
        released = false;
    }
}

public class PlayerAvatar : NetworkBehaviour
{
    // ステータス
    [Networked] public int m_hp { get; set; } = 3; // HP
    private int m_maxHp = 3; // 最大HP

    bool m_active = false;

    // プレイヤー名のネットワークプロパティを定義する
    [Networked] public NetworkString<_16> NickName { get; set; } // プレイヤー名のネットワークプロパティ

    [SerializeField] GameObject m_bullet; // テスト用の魔弾のプレハブを入れておく

    private NetworkMecanimAnimator m_animator; // アニメーター

    [Networked] private Vector3 m_startPos { get; set; } // 初期位置

    private float m_speed = 3.0f;
    private Vector3 m_moveVec = new Vector3(0.0f, 0.0f, 0.0f);
    private Quaternion m_rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

    [Networked] private int m_id { get; set; } = 0;

    [Networked] private bool m_restartFlag { get; set; } = false;
    [Networked] private bool m_beforReStartFlag { get; set; } = false;
    [Networked] bool m_reStartChackFlag { get; set; } = false; // リスタート完了しているか確認するフラグ

    // 角度関係
    protected Vector3 m_directionVec;
    protected float m_totalAngle = 0.0f; // カメラの角度と移動方向の角度を足したもの
    protected Vector3 m_moveValue;
    protected float m_playerAngle; // 移動方向の角度


    // インプットアクション
    protected Dictionary<string, InputAction> m_inputActions = new Dictionary<string, InputAction>();
    protected Dictionary<string, InputDeta> m_inputDetas = new Dictionary<string, InputDeta>();
    private InputAction m_moveAction;
    private InputAction m_dodgeAction;
    private InputAction m_dashAction;
    private InputAction m_attackAction;

    // 入力処理
    private bool m_attackPressed = false;
    private bool m_attackReleased = false;
    public bool m_dodgePressed = false;
    public bool m_dodgeReleased = false;

    GameObject m_camera;

    // 攻撃用のステートマシン
    //private StateMachineShot m_stateMachineAttack;
    // バトルマネージャー
    [SerializeField] private GameBattleManager m_battleManager;

    // 行動用のステートマシン
    private StateMachinePlayer m_stateMachinePlayer;

    // これがフォトンのビヘイビアにとってのStart()
    public override void Spawned()
    {
        m_startPos = transform.position; // 初期位置を保存

        m_id = Runner.SessionInfo.PlayerCount;

        //m_characterController = GetComponent<NetworkCharacterController>();
        m_animator = GetComponent<NetworkMecanimAnimator>();

        // MoveとJumpのアクションを取得
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_inputActions.Add("Move", m_moveAction);
        m_dodgeAction = InputSystem.actions.FindAction("Dodge");
        m_inputActions.Add("Dodge", m_dodgeAction);
        m_inputDetas.Add("Dodge", new InputDeta { pressed = false, released = false });
        m_dashAction = InputSystem.actions.FindAction("Dash");
        m_inputActions.Add("Dash", m_dashAction);
        m_attackAction = InputSystem.actions.FindAction("Attack");
        m_inputActions.Add("Attack", m_attackAction);
        m_inputDetas.Add("Attack", new InputDeta { pressed = false, released = false });

        // ↓これはInputSystemのアクションマップを使用しない場合の例
        // 直接コードでアクションを書くときはこうなる
        //m_moveAction = new InputAction("Move", binding: "<Gamepad>/leftStick");
        //m_jumpAction = new InputAction("Jump", binding: "<Gamepad>/buttonSouth");

        // カメラを取得
        m_camera = GameObject.Find("Camera");
        // HPUIに対し自分を渡す
        var hpUI = GameObject.Find("PlayerHPUI");
        hpUI.GetComponent<UIPlayerHP>().SetPlayer(gameObject);

        // バトルマネージャーに自分を渡す
        var battleManagerObj = GameObject.Find("GameBattleManager(Clone)");
        GameBattleManager battleManager = new GameBattleManager();
        //Debug.Log("バトルマネージャーのStateAuthorityは" + batteleManagerStateAuthorty);

        if (!battleManagerObj)
        {
            var test = Runner.Spawn(m_battleManager, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity); // バトルマネージャーを生成
            battleManager = test.GetComponent<GameBattleManager>();
        }
        else if(battleManagerObj)
        {
            battleManager = battleManagerObj.GetComponent<GameBattleManager>();
        }

        battleManager.SetPlayer(Object);

        // プレイヤー名をテキストに反映させる
        var view = GetComponent<PlayerAvatarView>();
        view.SetNickName(NickName.Value);

        //// 攻撃用のステートマシン作成
        //m_stateMachineAttack = new StateMachineShot(Object, m_bullet);

        // 行動用のステートマシン作成
        m_stateMachinePlayer = new StateMachinePlayer(Object, m_camera);

        // ボタンに対して自分を登録する
        var reStartButton = GameObject.Find("Canvas");
        Debug.Log(reStartButton);
        reStartButton.GetComponent<ReStartButton>().SetParentObject(Object);

        // 攻撃マネージャーに対して自分を登録する
        var attackManagerObj = GameObject.Find("AttackManager");
        attackManagerObj.GetComponent<AttackManager>().SetPlayer(Object);
    }

    // これがフォトンのビヘイビアにとってのUpdate()
    public override void FixedUpdateNetwork()
    {
        // バトルマネージャーがいない限り動けない
        // ゲームのフェーズを取得する
        var gameManager = GameObject.Find("GameBattleManager(Clone)").GetComponent<GameBattleManager>();
        var gamePhase = gameManager.GetGamePhase();

        //if (gamePhase <= E_GamePhase.Preparation)   return;
        if (!m_active) return;

        // デバック(この形で本実装するかも)
        if (gamePhase == E_GamePhase.Battle)
        {
            // リスタートフラグを初期化
            m_restartFlag = false;
        }

        float delta = Runner.DeltaTime; // ネットワークのデルタタイムを使用する

        // 移動反映
        transform.position += m_moveVec * delta;
        transform.rotation = m_rotation;

        // アニメーション
        var animator = m_animator.Animator;
        //var grounded = m_characterController.Grounded;
        //var vy = m_characterController.Velocity.y;
        Vector2 animationMove = new Vector2(m_moveVec.x, m_moveVec.z);
        animator.SetFloat("Speed", animationMove.magnitude * 50.0f);
        //animator.SetBool("Jump", !grounded && vy > 4.0f);
        //animator.SetBool("Grounded", grounded);
        //animator.SetBool("FreeFall", !grounded && vy < -4.0f);
        animator.SetFloat("MotionSpeed", 1.0f);

        // ステートマシンの更新
        //m_stateMachineAttack.OnUpdate();
        m_stateMachinePlayer.OnUpdate();
    }

    // フォトンのビヘイビアを使用する場合従来のStartやUpdateなどが使用できないので上の
    // Spawned()やFixedUpdateNetwork()などを使用する必要がある
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        // 瞬間の入力を保管する
        foreach (var inputDeta in m_inputDetas)
        {
            var inputName = inputDeta.Key;
            var inputStruct = inputDeta.Value;
            var inputAction = m_inputActions[inputName];

            if (inputAction.WasPressedThisFrame())
            {
                Debug.Log("入力されたよ");
                inputStruct.pressed = true;
                //m_inputDetas[inputName] = inputStruct; // 更新された構造体を戻す(更新させる)
            }

            if (inputAction.WasReleasedThisFrame())
            {
                Debug.Log("入力離されたよ");
                inputStruct.released = true;
                ///m_inputDetas[inputName] = inputStruct; // 更新された構造体を戻す(更新させる)
            }
        }


        // デバック用のif書きました
        if (m_dodgeAction.WasPressedThisFrame())
        {
            m_dodgePressed = true;
            Debug.Log("回避した");
        }

        if (m_dodgeAction.WasReleasedThisFrame())
        {
            m_dodgeReleased = true;
            Debug.Log("回避をやめた");
        }
    }

    public int HP
    {
        get { return m_hp; }
        set { m_hp = Mathf.Clamp(value, 0, m_maxHp); } // HPを0から最大HPの範囲に制限
    }

    public int Id 
    { 
        get { return m_id; } 
    }

    public Vector3 StartPos
    {
        get { return m_startPos; }
    }

    public bool RestartFlag
    {
        get { return m_restartFlag; }
        set { m_restartFlag = value; }
    }

    public bool beforReStartFlag
    {
        get { return m_beforReStartFlag; }
    }

    public bool RestartChackFlag
    {
        get { return m_reStartChackFlag; }
        set { m_reStartChackFlag = value; }
    }

    public bool Active 
    { 
        get { return m_active; }
        set { m_active = value; }
    }

    public Vector3 MoveVec
    {
        get { return m_moveVec; }
        set { m_moveVec = value; }
    }

    public Quaternion Rotation
    {
        get { return m_rotation; }
        set { m_rotation = value; }
    }

    // 方向ベクトル
    public Vector3 directionVec
    {
        get { return m_directionVec; }
        set { m_directionVec = value; }
    }

    // カメラの角度と移動方向の角度を足したもの
    public float totalAngle
    {
        get { return m_totalAngle; }
        set { m_totalAngle = value; }
    }

    // 移動ベクトル
    public Vector3 moveValue
    {
        get { return m_moveValue; }
        set { m_moveValue = value; }
    }

    // 移動方向の角度
    public float playerAngle
    {
        get { return m_playerAngle; }
        set { m_playerAngle = value; }
    }

    // 攻撃を受けるときの関数
    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void DamegeRpc(string attackName)
    {
        Debug.Log("テスト");
        // 攻撃マネージャー取得 このエラー確認方法はよろしくないので修正する予定
        var attackManagerObj = GameObject.Find("AttackManager");
        if (!attackManagerObj) return;
        var attackManager = attackManagerObj.GetComponent<AttackManager>();

        // 攻撃データの取得
        var attackDeta = attackManager.GetAttackDeta(attackName);

        Debug.Log("ダメージを受けた");
        m_hp -= attackDeta.attackPower;
    }

    public StateMachinePlayer GetStateMachinePlayer()
    {
        return m_stateMachinePlayer;
    }

    // ゲームリスタートの関数 RPCの呼び出しうまくいかない...
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void PlayerResetRpc()
    {
        m_hp = 3;
        transform.position = m_startPos;
        m_restartFlag = false;
        m_battleManager.GetComponent<GameBattleManager>().SetPlayer(Object);
        //m_battleManager.SetGamePhase(E_GamePhase.Start);
    }

    // コントローラー、キーマウの入力を取得する関数
    public Dictionary<string, InputAction> GetInputActions()
    {
        return m_inputActions;
    }

    // コントローラー、キーマウの入力を取得する関数(瞬間の処理用)
    public Dictionary<string, InputDeta> GetInputDetas()
    {
        return m_inputDetas;
    }

    // 入力の瞬間の処理を初期化する関数
    public void ResetInputDetas()
    {
        Dictionary<string, InputDeta> copyInputDetas = new Dictionary<string, InputDeta>(m_inputDetas); // コピーを作る   
        foreach (var inputDeta in copyInputDetas)
        {
            var inputName = inputDeta.Key;
            var inputStruct = inputDeta.Value;
            inputStruct.Reset();
            //m_inputDetas[inputName] = inputStruct; // 更新された構造体を戻す(更新させる)
        }
    }

    // 攻撃用オブジェクトを生成する関数
    //public void CreateAttackObject(HitInfo hitInfo,float activeTime)
    //{
    //    //Instantiate()

    //    //Runner.Spawn(attackObj, transform.position + transform.forward * 1.0f, transform.rotation, Object);
    //}

    //private void Active()
}
