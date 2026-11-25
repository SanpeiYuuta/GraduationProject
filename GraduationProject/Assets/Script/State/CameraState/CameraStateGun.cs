using UnityEngine;

public class CameraStateGun : CameraStateBase
{
    protected Vector3 m_pushMaxAtPos;

    public CameraStateGun(GameObject cameraObject) : base(cameraObject)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_range = 1.5f;
        m_height = 3.0f;

        m_pushMaxAtPos = new Vector3(-5.0f, -5.0f, -5.0f);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var cameraGunPos = new Vector3();
        cameraGunPos.x += Mathf.Cos(m_rad + 90.0f * Mathf.Deg2Rad) * 1.2f;
        cameraGunPos.z += Mathf.Sin(m_rad + 90.0f * Mathf.Deg2Rad) * 1.2f;

        var cameraPushVec = m_cameraDefaultVec + cameraGunPos;

        // プレイヤー取得
        var originObj = m_parentCamera.GetComponent<CameraMoveVer2>().GetOriginObject();
        var playerPos = originObj.transform.position;

        // カメラ差座標をカメラ本体に渡す
        m_differenceVec = cameraPushVec;
        m_parentCamera.GetComponent<CameraMoveVer2>().DifferenceVec = m_differenceVec;
        var directionVec = m_cameraDefaultVec.normalized;
        //m_stateAtPos = -directionVec * 3.0f;

        // レイ関係
        //m_stateAtPos = playerPos;
        m_stateAtPos.x = (Mathf.Cos(m_rad) * Mathf.Sin(m_radX) * m_pushMaxAtPos.x);
        m_stateAtPos.y = Mathf.Cos(m_radX) * m_pushMaxAtPos.y;
        m_stateAtPos.z = (Mathf.Sin(m_rad) * Mathf.Sin(m_radX) * m_pushMaxAtPos.z);

        Debug.Log("デフォルト差：" + m_stateAtPos);
        m_parentCamera.GetComponent<CameraMoveVer2>().PushAt = m_stateAtPos;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
