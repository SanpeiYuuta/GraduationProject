using Fusion;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 攻撃全般を管理するマネージャー
/// </summary>


public class AttackManager : MonoBehaviour
{
    // プレイヤーの一段目攻撃
    [SerializeField] GameObject m_playerAttackFirst;
    //[SerializeField] NetworkObject m_playerAttackFirstStartObj;
    // プレイヤーの二段目攻撃
    [SerializeField] GameObject m_playerAttackSecond;

    // 攻撃の情報を保管するためのプレハブ
    [SerializeField] NetworkObject m_playerAttackStartObj;

    // 攻撃を保管する配列
    private Dictionary<string, GameObject> m_attackArray = new Dictionary<string,GameObject>();
   // // 攻撃の発生を保管する配列
   // private Dictionary<string, NetworkObject> m_startAttackArray = new Dictionary<string, NetworkObject>();

    // プレイヤー
    private NetworkObject m_player;

    private void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // プレイヤー一段目攻撃登録
        string attackName = m_playerAttackFirst.GetComponent<DamageObject>().HitInfo.attackName;
        m_attackArray.Add(attackName, m_playerAttackFirst);

        // プレイヤー二段目攻撃登録
        attackName = m_playerAttackSecond.GetComponent<DamageObject>().HitInfo.attackName;
        m_attackArray.Add(attackName, m_playerAttackSecond);

        //// プレイヤー一段目攻撃の生成用オブジェクト登録
        //string attackStartObjName = m_playerAttackFirstStartObj.GetComponent<DamageObject>().HitInfo.attackName;
        //Debug.Log(attackStartObjName);
        //m_startAttackArray.Add(attackStartObjName, m_playerAttackFirstStartObj);

    }

    // Update is called once per frame
    void Update()
    {

    }

    // ステージに攻撃オブジェクトを生成する
    // attackName：生成したい攻撃オブジェクトの名前
    // createPos：生成した際の位置 デフォルト値(0,0,0)
    // createQt：生成した際の回転 デフォルト値null 実質Quaternion.identity
    public GameObject CreateAttackObject(string attackName,NetworkObject parentObj, Vector3 createPos = default, Quaternion? createQt = null)
    {
        // エラー
        if (!m_attackArray.TryGetValue(attackName, out var prefab))
        {
            Debug.LogWarning($"攻撃オブジェクトが見つかりません：{attackName}");
            return null;
        }

        // createQtがnullの場合はQuaternion.identityにする
        var qt = createQt ?? Quaternion.identity;
        var createObj = Instantiate(m_attackArray[attackName], createPos, qt);
        createObj.GetComponent<DamageObject>().SetParentObj(parentObj); // 親渡し

        return createObj;
    }

    // ステージに攻撃オブジェクトを生成する(ネットワーク版)
    // attackName：生成したい攻撃オブジェクトの名前
    // createPos：生成した際の位置 デフォルト値(0,0,0)
    // createQt：生成した際の回転 デフォルト値null 実質Quaternion.identity
    public NetworkObject CreateAttackObjectNetwork(string attackName,NetworkObject parentObj, Vector3 createPos = default, Quaternion? createQt = null)
    {
        // エラー
        if (!m_attackArray.TryGetValue(attackName, out var prefab))
        {
            Debug.LogWarning($"攻撃オブジェクトが見つかりません：{attackName}");
            return null;
        }

        // createQtがnullの場合はQuaternion.identityにする
        var qt = createQt ?? Quaternion.identity;

        var startAttackObj = m_player.Runner.Spawn(m_playerAttackStartObj, createPos, qt, onBeforeSpawned: (_, NetworkObject) =>
        {
            var attackStartObj = NetworkObject.GetComponent<AttackStartObj>();

            // 名前設定
            attackStartObj.AttackName = attackName;
            // 攻撃オブジェクトの情報を設定する
            attackStartObj.AttackName = attackName;
            attackStartObj.m_createPos = createPos;
            attackStartObj.m_createQt = qt;
            // 親オブジェクト渡す
            attackStartObj.ParentObj = parentObj;
        });
        //startAttackObj.GetComponent<AttackStartObj>().AttackName = "12345678901234567890";
        //startAttackObj.GetComponent<AttackStartObj>().AttackName += "1234567890";
        //startAttackObj.GetComponent<AttackStartObj>().AttackName = attackName;

       // Debug.Log($"attackName：{startAttackObj.GetComponent<AttackStartObj>().AttackName}");

        return startAttackObj;
    }

    // 攻撃プレハブの取得
    // 取得したい攻撃プレハブの名前
    public GameObject GetAttackPrefab(string attackName)
    {
        // エラー
        if (!m_attackArray.TryGetValue(attackName, out var prefab))
        {
            Debug.LogWarning($"攻撃プレハブが見つかりません：{attackName}");
            return null;
        }

        return m_attackArray[attackName];
    }

    // 攻撃データの取得
    // 取得したい攻撃データの名前
    public AttackDeta GetAttackDeta(string attackName)
    {
        // エラー
        if (!m_attackArray.TryGetValue(attackName, out var prefab))
        {
            Debug.LogWarning($"攻撃データが見つかりません：{attackName}");
            return null;
        }

        return m_attackArray[attackName].GetComponent<DamageObject>().HitInfo;
    }

    // プレイヤーを設定するセッタ
    public void SetPlayer(NetworkObject player)
    {
        // 既に設定されている場合は、再設定しない
        if (m_player) return;

        m_player = player;
    }

    public NetworkObject ParentObj
    {
        get { return m_player; }
    }
}
