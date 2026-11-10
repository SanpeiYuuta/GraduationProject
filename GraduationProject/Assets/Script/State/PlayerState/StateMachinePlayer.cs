using Fusion;
using UnityEngine;
/// <summary>
/// プレイヤーのステートマシン
/// </summary>

public class StateMachinePlayer : StateMachineBase
{

    // コンストラクタ
    public StateMachinePlayer(NetworkObject parentObject, GameObject cameraObject) : base(parentObject)
    {
        AddState("Walk", new PlayerStateWalk(m_parentObject, cameraObject));
        AddState("Dodge", new PlayerStateDodge(m_parentObject, cameraObject));
        AddState("AttackFirst", new PlayerStateAttackFirst(m_parentObject, cameraObject));
        AddState("AttackSecond", new PlayerStateAttackSecond(m_parentObject, cameraObject));
        ChangeState("Walk");
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

}
