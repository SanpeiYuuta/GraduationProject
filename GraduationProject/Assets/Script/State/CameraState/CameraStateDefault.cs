using UnityEngine;
/// <summary>
/// ステートの役割は注視点と座標がどこになるか決めるだけ
/// </summary>
public class CameraStateDefault : CameraStateBase
{
    public CameraStateDefault(GameObject cameraObject):base(cameraObject)
    {
        //m_stateEyePos = 
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var cameraPushVec = m_cameraDefaultVec;

        // プレイヤー取得
        var originObj = m_parentCamera.GetComponent<CameraMoveVer2>().GetOriginObject();
        var playerPos = originObj.transform.position;

        // カメラ差座標をカメラ本体に渡す
        m_differenceVec = cameraPushVec;
        m_parentCamera.GetComponent<CameraMoveVer2>().DifferenceVec = m_differenceVec;
        m_stateAtPos = -m_cameraDefaultVec * 1.0f;
        m_parentCamera.GetComponent<CameraMoveVer2>().PushAt = m_stateAtPos;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}
