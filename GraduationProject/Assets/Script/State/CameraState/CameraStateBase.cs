using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// カメラのステートの基盤となるクラス
/// </summary>

public class CameraStateBase : StateBase
{
    // インプットアクション
    private InputAction m_resetAngle;
    private InputAction m_cameraMove;

    // カメラオブジェクト
    protected GameObject m_parentCamera;
    //// デルタタイム
    //protected float m_deltaTime;

    // 目標注視点
    protected Vector3 m_stateAtPos;
    //// 現在注視点
    //protected Vector3 m_currentAtPos;
    //// 目標座標
    //protected Vector3 m_stateEyePos;
    //// 現在座標
    //protected Vector3 m_currentEyePos;

    // 自機との差ベクトル
    protected Vector3 m_differenceVec;
    //// 
    //protected Vector3 m_pushAt;

    // フィルターをかけないで自機から見たカメラの差の座標
    protected Vector3 m_cameraDefaultVec;

    // ここから下の変数はカメラ本体のメンバ変数と同期させないといけない
    protected float m_rad;//ラジアン角
    protected float m_radX; //ラジアン角X回転

    protected float m_range; // 自機と離れる距離
    protected float m_height; // 自機との高さの差

    // カメラの移動スピード
    protected float m_speed;

    Vector2 m_limitRadX; // ラジアンX回転の制限(X：下限 Y：上限)ディグリー角で書いてる

    public CameraStateBase(GameObject cameraObject) : base(cameraObject)
    {
        // 親オブジェクト設定
        m_parentCamera = cameraObject;
        m_speed = m_parentCamera.GetComponent<CameraMoveVer2>().Speed;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // カメラ移動アクションを取得
        m_cameraMove = InputSystem.actions.FindAction("CameraMove");
        m_range = 5.0f;
        m_height = 5.0f;
        m_limitRadX = new Vector2(60.0f, 120.0f); // radX制限決める

        m_rad = m_parentCamera.GetComponent<CameraMoveVer2>().rad;
        m_radX = m_parentCamera.GetComponent<CameraMoveVer2>().RadX;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var cameraMove = m_parentCamera.GetComponent<CameraMoveVer2>();

        //スティックの傾きによって角度を追加する
        var cameraMoveValue = m_cameraMove.ReadValue<Vector2>();
        float addAngle = -cameraMoveValue.x; // 右スティックの右左の入力を取得
        float addAngleY = cameraMoveValue.y; // 右スティックの上下入力を取得

        //スティックのデットゾーンを決める
        addAngle = DeadZone(addAngle);
        addAngleY = DeadZone(addAngleY);

        //スティックの傾きによってカメラの向きを回転させる
        float angle = (addAngle * m_speed);
        float angleY = (addAngleY * m_speed);
        m_rad += angle * m_deltaTime;
        m_radX += angleY * m_deltaTime;

        //カメラ回転制限
        if (m_limitRadX.x * Mathf.Deg2Rad > m_radX)
        {
            // 下限制限
            m_radX = m_limitRadX.x * Mathf.Deg2Rad;
        }
        else if (m_limitRadX.y * Mathf.Deg2Rad < m_radX)
        {
            // 上限制限
            m_radX = m_limitRadX.y * Mathf.Deg2Rad;
        }

        // レイ関係
        m_cameraDefaultVec.x = (Mathf.Cos(m_rad) * Mathf.Sin(m_radX) * m_range);
        m_cameraDefaultVec.y = Mathf.Cos(m_radX) * m_height;
        m_cameraDefaultVec.z = (Mathf.Sin(m_rad) * Mathf.Sin(m_radX) * m_range);

        // 自機に対してのカメラがいる方向を本体に渡しておく
        cameraMove.rad = m_rad;
        cameraMove.RadX = m_radX;
    }

    public override void OnExit()
    {

    }

    //スティックのデットゾーンを決める
    float DeadZone(float addAngle)
    {
        //スティックのデットゾーンを決める
        if (Mathf.Abs(addAngle) <= 0.3f)
        {
            addAngle = 0;
        }

        return addAngle;
    }

}
