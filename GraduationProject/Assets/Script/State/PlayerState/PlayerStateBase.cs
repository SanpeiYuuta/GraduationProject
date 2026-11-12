using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// プレイヤー用のステートベースクラス　これを親としてプレイヤー用のステートを作る
/// </summary>

public abstract class PlayerStateBase : StateBase
{
    // 親オブジェクトアバター
    protected PlayerAvatar m_playerAvatar;

    protected Dictionary<string, InputAction> m_playerInputActions = new Dictionary<string, InputAction>();
    protected Dictionary<string, InputDeta> m_playerInputDetas = new Dictionary<string, InputDeta>();

    GameObject m_camera; // カメラ

    // 動く方向ベクトル
    protected Vector3 m_directionVec;
    protected float m_totalAngle = 0.0f; // カメラの角度と移動方向の角度を足したもの
    protected Vector3 m_moveValue;
    protected float m_playerAngle; // 移動方向の角度

    public PlayerStateBase(NetworkObject parentObject,GameObject cameraObject) : base(parentObject)
    {
        m_camera = cameraObject;
    }

    public override void OnEnter()
    {
        // プレイヤー操作用のInputActionを取得
        m_playerAvatar = m_parentObject.GetComponent<PlayerAvatar>();
        m_playerInputActions = m_playerAvatar.GetInputActions();
        m_playerInputDetas = m_playerAvatar.GetInputDetas();

        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var playerAvatar = m_parentObject.GetComponent<PlayerAvatar>();

        // 移動
        float cameraAngle = m_camera.GetComponent<CameraMove>().GetAngle(); // カメラの角度を渡す
        //Vector2 cameraValue = new Vector2(cameraForward.x, cameraForward.z).normalized;

        m_moveValue = m_playerInputActions["Move"].ReadValue<Vector2>();

        if(m_moveValue != new Vector3(0.0f,0.0f,0.0f))
        {
            m_playerAngle = Mathf.Atan2(m_moveValue.x, m_moveValue.y);
            playerAvatar.playerAngle = m_playerAngle;
            //Debug.Log("m_playerAngle: " + m_playerAngle * Mathf.Rad2Deg);
        }

        m_totalAngle = m_playerAngle - cameraAngle;
        playerAvatar.totalAngle = m_totalAngle;
        //Debug.Log("m_totalAngle: " + m_totalAngle * Mathf.Rad2Deg);
        // 動く方向の決定
        m_directionVec = new Vector3(Mathf.Cos(m_totalAngle), 0.0f, -Mathf.Sin(m_totalAngle));
        //Debug.Log("m_totalAngle: " + m_totalAngle*Mathf.Rad2Deg);
        m_directionVec.Normalize();
        playerAvatar.directionVec = m_directionVec;

    }

    public override void OnExit()
    {
        base.OnExit();
    }

}
