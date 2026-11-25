using Fusion;
using UnityEngine;
/// <summary>
/// プレイヤーのステートマシン
/// </summary>

public class StateMachinePlayer : NetObjStateMachineBase
{

    // コンストラクタ
    public StateMachinePlayer(NetworkObject parentObject, GameObject cameraObject) : base(parentObject)
    {
        AddState("Walk", new PlayerStateWalk(m_parentObject, cameraObject));
        AddState("Dodge", new PlayerStateDodge(m_parentObject, cameraObject));
        // 刀攻撃(予定)
        AddState("AttackFirst", new PlayerStateAttackFirst(m_parentObject, cameraObject));
        AddState("AttackSecond", new PlayerStateAttackSecond(m_parentObject, cameraObject));
        AddState("AttackThird", new PlayerStateAttackThird(m_parentObject, cameraObject));
        // ダガー攻撃
        AddState("DaggerFirst", new DaggerFirst(m_parentObject, cameraObject));
        AddState("DaggerSecond", new DaggerSecond(m_parentObject, cameraObject));
        AddState("DaggerThird", new DaggerThird(m_parentObject, cameraObject));
        // 大剣攻撃
        AddState("GreatswordFirst", new GreatswordFirst(m_parentObject, cameraObject));
        AddState("GreatswordSecond", new GreatswordSecond(m_parentObject, cameraObject));
        AddState("GreatswordThird", new GreatswordThird(m_parentObject, cameraObject));
        // シューター攻撃
        AddState("BulletAttack", new StateShot(m_parentObject, cameraObject));
        // ハンドガン腰打ち攻撃
        AddState("HandgunHipShot", new HandgunHipShotState(m_parentObject, cameraObject));
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
