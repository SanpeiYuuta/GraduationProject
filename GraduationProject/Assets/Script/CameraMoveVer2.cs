using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMoveVer2 : MonoBehaviour
{
    [SerializeField] float m_range;//追尾距離
    [SerializeField] float m_speed;//旋回スピード
    [SerializeField] float m_height;//カメラの高さ

    [SerializeField] GameObject m_originObject;//旋回する際の中心のオブジェクト
    [SerializeField] GameObject m_cameraObject;//カメラのオブジェクト

    // インプットアクション
    private InputAction m_resetAngle;
    private InputAction m_cameraMove;

    private Vector3 m_targetVec;//ターゲットとプレイヤーとの距離

    [SerializeField] float m_deg;//中心点から角度の方向にいるのかを管理する
    float m_rad;//ラジアン角
    float m_radX; //ラジアン角X回転
    float m_targetDeg;//自分から見てターゲットの角度

    bool m_lockOn;//ロックオンするかしないかの変数
    bool m_beforPlayerCamerafllow = false;

    Vector3 m_differenceVec; // 自機との距離差
    Vector3 m_cameraDefaultVec;
    Vector3 originPos;
    Vector3 m_pushAt; // 注視点の差

    Vector3 m_atPos; // 注視点座標
    Vector3 m_eyePos; // 位置座標

    // ステートマシン
    CameraStateMachine m_cameraStateMachine;

    // Start is called before the first frame update
    void Start()
    {
        m_resetAngle = InputSystem.actions.FindAction("ResetAngle"); // リセットアクションを取得
        m_cameraMove = InputSystem.actions.FindAction("CameraMove"); // カメラ移動アクションを取得

        m_rad = m_deg * Mathf.Deg2Rad;

        // ステートマシン作成
        m_cameraStateMachine = new CameraStateMachine(gameObject);
    }

    private void Update()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 追跡対象がいなければ処理を行わない
        if (!m_originObject) return;

        // プレイヤーの移動方法からカメラの配置変更します
        bool playerMoveType = m_originObject.GetComponent<PlayerAvatar>().CameraFllow;
        if (playerMoveType != m_beforPlayerCamerafllow)
        {
            if (playerMoveType)
            {
                m_cameraStateMachine.ChangeState("Gun");
            }
            else
            {
                m_cameraStateMachine.ChangeState("Default");
            }
            m_beforPlayerCamerafllow = playerMoveType;
        }

        // ステートマシン更新
        m_cameraStateMachine.OnUpdate();

        //中心点の座標取得して自分もその座標にする
        originPos = m_originObject.transform.position;
        //originPos.y = m_height;
        ////transform.position = originPos;

        ////デルタタイム取得
        //float delta = Time.deltaTime;
        ////スティックの傾きによって角度を追加する
        //var cameraMoveValue = m_cameraMove.ReadValue<Vector2>();
        //float addAngle = -cameraMoveValue.x; // 右スティックの右左の入力を取得
        //float addAngleY = cameraMoveValue.y; // 右スティックの上下入力を取得

        ////スティックのデットゾーンを決める
        //addAngle = DeadZone(addAngle);
        //addAngleY = DeadZone(addAngleY);

        ////スティックの傾きによってカメラの向きを回転させる
        //float angle = (addAngle * m_speed);
        //float angleY = (addAngleY * m_speed);
        //m_rad += angle * delta;
        //m_radX += angleY * delta;
        //Debug.Log("radX：" + m_radX);
        ////var cameraRot = transform.eulerAngles;
        ////cameraRot += new Vector3(angleY, angle) * delta;
        ////Debug.Log("カメラのX回転:" + m_radX);

        ////カメラ回転制限
        //if (60.0f * Mathf.Deg2Rad > m_radX)
        //{
        //    m_radX = 60.0f * Mathf.Deg2Rad;
        //}
        //else if (120.0f * Mathf.Deg2Rad < m_radX)
        //{
        //    m_radX = 120.0f * Mathf.Deg2Rad;
        //}

        //// レイ関係
        //m_cameraDefaultVec.x = (Mathf.Cos(m_rad) * Mathf.Sin(m_radX) * m_range);
        //m_cameraDefaultVec.y = Mathf.Cos(m_radX) * m_height;
        //m_cameraDefaultVec.z = (Mathf.Sin(m_rad) * Mathf.Sin(m_radX) * m_range);

        //// 銃モードの離れる距離プラス
        //var cameraGunPos = new Vector3();
        //cameraGunPos.x += Mathf.Cos(m_rad + 90.0f * Mathf.Deg2Rad) * 3.0f;
        //cameraGunPos.z += Mathf.Sin(m_rad + 90.0f * Mathf.Deg2Rad) * 3.0f;

        //m_differenceVec = m_cameraDefaultVec + cameraGunPos;

        //float rayDistance = m_differenceVec.magnitude;

        // レイを出す
        Debug.DrawRay(originPos, m_differenceVec, new Color(0.0f, 1.0f, 0.0f, 1.0f));
        Ray ray = new Ray(originPos, m_differenceVec);

        // 移動する予定の場所が障害物に当たっていたらその手前の位置に移動する
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, m_differenceVec.magnitude))
        {
            Vector3 pushVec = m_differenceVec;

            var hitObj = rayHit.collider.gameObject;
            if (hitObj.tag == "Obstacle")
            {
                pushVec = rayHit.point;
                m_cameraObject.GetComponent<Transform>().position = pushVec;
            }
            else
            {
                // 中心点からrangeの数値分距離を離す
                m_cameraObject.GetComponent<Transform>().position = originPos + pushVec;
            }

            //m_pushAt = -m_cameraDefaultVec * 1.0f;
            m_atPos = m_originObject.transform.position + m_pushAt;
            m_cameraObject.transform.LookAt(m_atPos);
        }
        else
        {
            //m_eyePos = cameraPos;

            // 中心点からrangeの数値分距離を離す
            m_cameraObject.GetComponent<Transform>().position = originPos + m_differenceVec;

            m_atPos = m_originObject.transform.position;
            //m_pushAt = -m_cameraDefaultVec * 1.0f;
            //Debug.Log("デフォルト差：" + m_pushAt);
            m_cameraObject.transform.LookAt(m_atPos + m_pushAt);
        }

        //スティックの押し込みでPlayerの向いている方向に回り込む
        // MovePlayerAngle(); ←実装してないよ
    }

    //回り込んでいる角度を渡す
    public float GetAngle()
    {
        Vector3 Vector = m_originObject.transform.position - (transform.position);//方向ベクトルを取得する
        float rotAngle = Mathf.Atan2(Vector.z, Vector.x);//角度に変換
        return rotAngle;
    }

    //Playerの向いている方向に回り込む
    private void MovePlayerAngle()
    {
        // プレイヤーの向いている方向を角度として取得する//
        Vector3 playerforward = m_originObject.transform.forward;
        //スティックの押し込みでPlayerの向いている方向に旋回する
        //Playerの回転を受け取る
        float playerRad = Mathf.Atan2(playerforward.z, playerforward.x);

        //RBを押し込んだらプレイヤーの向いている方向にカメラが向くようにする
        //if (Input.GetKeyDown("joystick button 5"))
        //{
        //    float resetRad = (playerRad);
        //    float resetDeg = (playerRad * 180 / Mathf.PI);//ラジアンから度数法へ
        //    //回転更新
        //    transform.eulerAngles = new Vector3(0.0f, -(resetDeg));
        //}
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

    //現在ロックオンしているか確認するゲッター
    public bool GetLockOn()
    {
        return m_lockOn;
    }

    //ターゲットとプレイヤーとの距離のゲッター
    public Vector3 GetTargetVec()
    {
        return m_targetVec;
    }

    ////自分から見てターゲットの角度のゲッター
    public float GetTargetDeg()
    {
        return m_targetDeg;
    }

    // 追跡対象を設定するセッター
    public void SetOriginObject(GameObject originObject)
    {
        // すでに追跡対象が設定されている場合は、再設定しない
        if (m_originObject) return;

        m_originObject = originObject;
    }

    // 中心のオブジェクトのゲッタ
    public GameObject GetOriginObject()
    {
        if(!m_originObject) return null;

        return m_originObject;
    }

    // Y軸回転
    public float rad
    {
        get { return m_rad; }
        set { m_rad = value; }
    }

    // radXのゲッタセッタ
    public float RadX
    {
        get { return m_radX; }
        set { m_radX = value; }
    }

    // 注視点座標
    public Vector3 AtPos
    {
        get { return m_atPos; }
        set { m_atPos = value; }
    }

    // 位置座標
    public Vector3 EyePos
    {
        get { return m_eyePos; }
        set { m_eyePos = value; }
    }

    // 自機との位置差
    public Vector3 DifferenceVec
    {
        get { return m_differenceVec; }
        set { m_differenceVec = value; }
    }

    // 注視点の差
    public Vector3 PushAt
    {
        get { return m_pushAt; }
        set { m_pushAt = value; }
    }

    // カメラの移動スピード
    public float Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }

}
