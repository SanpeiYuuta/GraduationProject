using Fusion;
using UnityEngine;
/// <summary>
/// ネットワーク上で動く状態を管理する基底クラス(ステート)
/// </summary>


public class NetStateBase
{
    // 親オブジェクト
    protected NetworkObject m_parentObject;

    // デルタタイム 
    protected float m_deltaTime;

    // コンストラクタ
    public NetStateBase(NetworkObject parentObject)
    {
        m_parentObject = parentObject;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void OnEnter()
    {

    }

    // Update is called once per frame
    public virtual void OnUpdate()
    {
        m_deltaTime = m_parentObject.Runner.DeltaTime;
    }

    public virtual void OnExit()
    {

    }
}
