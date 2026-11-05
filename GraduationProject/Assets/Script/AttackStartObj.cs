using Fusion;
using UnityEngine;
/// <summary>
/// ダメージオブジェクトを生成するためのネットワークオブジェクト
/// </summary>

public class AttackStartObj : NetworkBehaviour
{
    // 攻撃オブジェクトを生成したかフラグ
    bool m_isCreateFlag = false;

    string m_attackName = null;
    Vector3 m_createPos = Vector3.zero;
    Quaternion m_createQt = Quaternion.identity;

    // 親オブジェクト
    NetworkObject m_parentObj = null;

    // 生成した攻撃オブジェクト
    GameObject m_attackObj = null;

    public override void Spawned()
    {
        m_createPos = transform.position;
        m_createQt = transform.rotation;

        // 生成されたとたん攻撃オブジェクトを生成する
        CreateAttackObject(m_attackName, m_createPos, m_createQt);
        m_attackObj.GetComponent<DamageObject>().SetParentObj(m_parentObj);
    }

    public override void FixedUpdateNetwork()
    {
        if(m_isCreateFlag)  Destroy(gameObject);
    }

    // ステージに攻撃オブジェクトを生成する
    // attackName：生成したい攻撃オブジェクトの名前
    // createPos：生成した際の位置 デフォルト値(0,0,0)
    // createQt：生成した際の回転 デフォルト値null 実質Quaternion.identity
    public GameObject CreateAttackObject(string attackName, Vector3 createPos = default, Quaternion? createQt = null)
    {
        // 攻撃マネージャー取得
        var attackManager = GameObject.Find("AttackManager");

        // createQtがnullの場合はQuaternion.identityにする
        var qt = createQt ?? Quaternion.identity;

        m_isCreateFlag = true; // 生成完了
        m_attackObj = attackManager.GetComponent<AttackManager>().CreateAttackObject(attackName, createPos, createQt);
        return m_attackObj;
    }

    public string AttackName
    {
        get { return m_attackName; } 
        set { m_attackName = value; }
    }

    public NetworkObject ParentObj
    {
        get { return m_parentObj; }
        set { m_parentObj = value; }
    }
}
