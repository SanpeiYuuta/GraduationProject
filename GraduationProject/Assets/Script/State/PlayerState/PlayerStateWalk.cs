using Fusion;
using UnityEngine;

public class PlayerStateWalk : PlayerStateBase
{
    float m_speed = 3.0f;

    public PlayerStateWalk(NetworkObject parentObject,GameObject cameraObject) : base(parentObject, cameraObject)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 回避状態移行
        var dodgeAction = m_playerInputActions["Dodge"];
        if(dodgeAction.IsPressed())
        {
            Debug.Log("回避状態に移行");
            var myState = m_parentObject.GetComponent<PlayerAvatar>().GetStateMachinePlayer();
            myState.ChangeState("Dodge");
            return;
        }

        // 攻撃状態移行
        var attackAction = m_playerInputActions["Attack"];
        if (attackAction.IsPressed())
        {
            Debug.Log("攻撃状態に移行");
            var myState = m_parentObject.GetComponent<PlayerAvatar>().GetStateMachinePlayer();
            myState.ChangeState("AttackFirst");
            return;
        }


        var dashAction = m_playerInputActions["Dash"];

        // 移動処理
        Vector3 move = new Vector3(0.0f, 0.0f, 0.0f);
        Quaternion direction = new Quaternion(1.0f,1.0f,1.0f,1.0f);
        var controllerVec = m_playerInputActions["Move"].ReadValue<Vector2>();
        //Debug.Log("m_directionVec: " + m_directionVec);
        if (dashAction.IsPressed() && controllerVec != new Vector2(0.0f,0.0f))
        {
            // ダッシュ用のスピードの調整したのち移動更新
            move = m_directionVec * (m_speed * 3.0f);
            direction = Quaternion.Euler(0.0f, (m_totalAngle * Mathf.Rad2Deg) + 90.0f, 0.0f);
            m_parentObject.GetComponent<PlayerAvatar>().Rotation = direction;
        }
        else if(controllerVec != new Vector2(0.0f, 0.0f))
        {
            // 徒歩用のスピードの調整したのち移動更新
            move = m_directionVec * m_speed;
            direction = Quaternion.Euler(0.0f, (m_totalAngle * Mathf.Rad2Deg) + 90.0f, 0.0f);
            m_parentObject.GetComponent<PlayerAvatar>().Rotation = direction;
        }

        // 移動反映
        m_parentObject.GetComponent<PlayerAvatar>().MoveVec = move;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
