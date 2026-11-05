using Fusion;
using UnityEngine;

public class PlayerStateDodge : PlayerStateBase
{
    private float m_countDodgeTime = 0.0f; // 回避のカウント時間

    float m_startTotalAngle; // 回避開始時の移動方向の角度

    public PlayerStateDodge(NetworkObject networkObject, GameObject cameraObject):base(networkObject,cameraObject)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        // 初期化
        m_countDodgeTime = 0.0f;

        var playerAvater = m_parentObject.GetComponent<PlayerAvatar>();

        // 回避開始時の移動方向ベクトルを保存
        m_startTotalAngle = playerAvater.totalAngle;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        float delta = m_parentObject.Runner.DeltaTime;

        m_countDodgeTime += delta*8.0f;

        // 回避の時間が経過したら歩行状態に戻す
        var dodgeTimeEnd = Mathf.PI / 2;
        if (m_countDodgeTime > dodgeTimeEnd)
        {
            m_parentObject.GetComponent<PlayerAvatar>().GetStateMachinePlayer().ChangeState("Walk");
            return;
        }

        // 回避のスピード曲線
        var cosCurve = Mathf.Cos(m_countDodgeTime);

        // 二次関数的な動きで回避行動する
        // 向いている方向に前方回避
        float dodgeSpeed = 30.0f;
        Vector3 moveVec = new Vector3(0.0f, 0.0f, 0.0f);
        moveVec.x = Mathf.Cos(m_startTotalAngle) * dodgeSpeed * cosCurve;
        moveVec.z = -Mathf.Sin(m_startTotalAngle) * dodgeSpeed * cosCurve;

        m_parentObject.GetComponent<PlayerAvatar>().MoveVec = moveVec;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
