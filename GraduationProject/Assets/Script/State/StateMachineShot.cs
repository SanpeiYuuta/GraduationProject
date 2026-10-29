using Fusion;
using UnityEngine;

public class StateMachineShot : StateMachineBase
{
    protected GameObject m_bulletObj;

    // コンストラクタ
    public StateMachineShot(NetworkObject parentObject,GameObject bulletObj) : base(parentObject)
    {
        m_bulletObj = bulletObj;
        AddState("Shot", new StateShot(m_parentObject, m_bulletObj));
        ChangeState("Shot");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnStart()
    {

    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        // 親クラスの更新呼び出し
        base.OnUpdate();
    }
}
