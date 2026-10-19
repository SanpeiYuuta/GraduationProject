using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerAvatar : NetworkBehaviour
{
    // ステータス
    private int m_hp = 3; // HP
    private int m_maxHp = 3; // 最大HP

    // プレイヤー名のネットワークプロパティを定義する
    [Networked] public NetworkString<_16> NickName { get; set; } // プレイヤー名のネットワークプロパティ

    [SerializeField] GameObject m_test;

    private NetworkCharacterController m_characterController; // キャラクターコントローラー
    private NetworkMecanimAnimator m_animator; // アニメーター

    private float m_speed = 3.0f;

    // インプットアクション
    private InputAction m_moveAction;
    private InputAction m_jumpAction;
    private InputAction m_dashAction;

    GameObject m_camera;

    // これがフォトンのビヘイビアにとってのStart()
    public override void Spawned()
    {
        m_characterController = GetComponent<NetworkCharacterController>();
        m_animator = GetComponent<NetworkMecanimAnimator>();

        // MoveとJumpのアクションを取得
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_jumpAction = InputSystem.actions.FindAction("Jump");
        m_dashAction = InputSystem.actions.FindAction("Dash");

        // ↓これはInputSystemのアクションマップを使用しない場合の例
        // 直接コードでアクションを書くときはこうなる
        //m_moveAction = new InputAction("Move", binding: "<Gamepad>/leftStick");
        //m_jumpAction = new InputAction("Jump", binding: "<Gamepad>/buttonSouth");

        // カメラを取得
        m_camera = GameObject.Find("Camera");
        //if (m_camera != null)
        //{
        //    m_camera.GetComponent<CameraMove>().SetOriginObject(gameObject);
        //}

        // プレイヤー名をテキストに反映させる
        var view = GetComponent<PlayerAvatarView>();
        view.SetNickName(NickName.Value);


    }

    // これがフォトンのビヘイビアにとってのUpdate()
    public override void FixedUpdateNetwork()
    {
        Debug.Log("体力：" + m_hp);

        // 体力0になった時の処理
        if (m_hp <= 0)
        {
            Transform model = transform.Find("Armature");
            model.gameObject.SetActive(false); // モデルを非表示にする(更新もしない)
            return;
        }

        float delta = Time.deltaTime;

        // 移動
        float cameraAngle = m_camera.GetComponent<CameraMove>().GetAngle(); // カメラの角度を渡す
        //Vector2 cameraValue = new Vector2(cameraForward.x, cameraForward.z).normalized;

        var moveValue = m_moveAction.ReadValue<Vector2>();
        float moveAngle = Mathf.Atan2(moveValue.x, moveValue.y);

        var totalMoveAngle = moveAngle - cameraAngle;
        //var move = new Vector3(totalMoveValue.x, 0.0f, totalMoveValue.y) * m_speed * delta;
        var moveVec = new Vector3(Mathf.Cos(totalMoveAngle), 0.0f, -Mathf.Sin(totalMoveAngle));

        //Debug.Log("向いている角度：" + totalMoveAngle);

        Vector3 move = new Vector3(0.0f, 0.0f, 0.0f);
        if (m_dashAction.IsPressed() && (moveValue.x != 0.0f || moveValue.y != 0.0f))
        {
            move = new Vector3(moveVec.x, 0.0f, moveVec.z) * (m_speed * 3.0f);
            transform.position += move * delta;
            //m_characterController.Move(move);
            transform.rotation = Quaternion.Euler(0.0f, (totalMoveAngle * Mathf.Rad2Deg) + 90.0f, 0.0f);
        }
        else if (moveValue.x != 0.0f || moveValue.y != 0.0f)
        {
            move = new Vector3(moveVec.x, 0.0f, moveVec.z) * m_speed;
            transform.position += move * delta;
            //m_characterController.Move(move * delta);
            transform.rotation = Quaternion.Euler(0.0f, (totalMoveAngle * Mathf.Rad2Deg) + 90.0f, 0.0f);
        }

        // ジャンプ
        if (m_jumpAction.IsPressed())
        {
            m_characterController.Jump();
        }

        // アニメーション
        var animator = m_animator.Animator;
        var grounded = m_characterController.Grounded;
        var vy = m_characterController.Velocity.y;
        Vector2 animationMove = new Vector2(move.x, move.z);
        animator.SetFloat("Speed", animationMove.magnitude * 50.0f);
        animator.SetBool("Jump", !grounded && vy > 4.0f);
        animator.SetBool("Grounded", grounded);
        animator.SetBool("FreeFall", !grounded && vy < -4.0f);
        animator.SetFloat("MotionSpeed", 1.0f);
    }



    // フォトンのビヘイビアを使用する場合従来のStartやUpdateなどが使用できないので上の
    // Spawned()やFixedUpdateNetwork()などを使用する必要がある
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public int HP
    {
        get { return m_hp; }
        set { m_hp = Mathf.Clamp(value, 0, m_maxHp); } // HPを0から最大HPの範囲に制限
    }
}
