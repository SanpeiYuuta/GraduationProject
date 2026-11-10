using Fusion;
using UnityEngine;
/// <summary>
/// ダメージオブジェクトを生成するためのネットワークオブジェクト
/// </summary>

public class AttackStartObj : NetworkBehaviour
{
    // 攻撃オブジェクトを生成したかフラグ
    bool m_isCreateFlag = false;

    [Networked,Capacity(32)] public string m_attackName { get; set; } = null;
    [Networked] public Vector3 m_createPos { get; set; } = Vector3.zero;
    [Networked] public Quaternion m_createQt { get; set; } = Quaternion.identity;

    // 親オブジェクト
    [Networked] NetworkObject m_parentObj { get; set; } = null;

    // 生成した攻撃オブジェクト
    GameObject m_attackObj = null;

    public override void Spawned()
    {
        // 生成されたとたん攻撃オブジェクトを生成する
        CreateAttackObject();
    }

    public override void FixedUpdateNetwork()
    {
        if (m_isCreateFlag)
        {
            Debug.Log("初期攻撃オブジェクトを消すよ");
            Runner.Despawn(Object);
        }
    }

    // ステージに攻撃オブジェクトを生成する
    // attackName：生成したい攻撃オブジェクトの名前
    // createPos：生成した際の位置 デフォルト値(0,0,0)
    // createQt：生成した際の回転 デフォルト値null 実質Quaternion.identity
    public GameObject CreateAttackObject()
    {
        // 攻撃マネージャー取得
        var attackManager = GameObject.Find("AttackManager");

        m_isCreateFlag = true; // 生成完了
        m_attackObj = attackManager.GetComponent<AttackManager>().CreateAttackObject(m_attackName, m_parentObj, m_createPos, m_createQt);
        m_attackObj.GetComponent<DamageObject>().SetParentObj(m_parentObj); // 親渡し
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
