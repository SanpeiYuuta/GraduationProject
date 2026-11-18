using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMoveVer2 : MonoBehaviour
{
    [SerializeField] float m_range;//追尾距離
    [SerializeField] float m_speed;//旋回スピード
    [SerializeField] float m_height;//カメラの高さ

    [SerializeField] GameObject m_originObject;//旋回する際の中心のオブジェクト
    [SerializeField] GameObject m_cameraObject;//カメラのオブジェクト
    //[SerializeField] GameObject m_targetRange; //ターゲットの範囲を司るオブジェクト

    // インプットアクション
    private InputAction m_resetAngle;
    private InputAction m_cameraMove;

    private Vector3 m_targetVec;//ターゲットとプレイヤーとの距離

    [SerializeField] float m_deg;//中心点から角度の方向にいるのかを管理する
    float m_rad;//ラジアン角
    float m_radX; //ラジアン角X回転
    float m_targetDeg;//自分から見てターゲットの角度
    float m_angleX = 0.0f; // Xの回転角度

    bool m_lockOn;//ロックオンするかしないかの変数
    bool m_beforeOriginObject = false;//前フレームでオリジンがちゃんと設定されているかの変数

    Vector3 pos;
    Vector3 originPos;

    Quaternion originRot;//元の回転度を保存する所

    // Start is called before the first frame update
    void Start()
    {
        m_resetAngle = InputSystem.actions.FindAction("ResetAngle"); // リセットアクションを取得
        m_cameraMove = InputSystem.actions.FindAction("CameraMove"); // カメラ移動アクションを取得

        //m_lockOn = false;//自動的にロックオンしないようにする
        //pos = GetComponent<Transform>().position;
        //originRot = m_cameraObject.GetComponent<Transform>().rotation;//カメラの初期回転を取得

        //m_rad = m_deg * Mathf.PI / 180;//ディグリーをラジアンにする

        ////中心点の座標取得して自分もその座標にする
        //originPos = m_originObject.GetComponent<Transform>().position;
        //originPos.y = m_height;
        //GetComponent<Transform>().position = originPos;


        ////中心点からrangeの数値分距離を離す
        //pos.x = originPos.x + (Mathf.Cos(m_rad) * m_range);
        //pos.z = originPos.z + (Mathf.Sin(m_rad) * m_range);
        //pos.y = m_height;

        ////変更した数値をカメラに適応させる
        //m_cameraObject.GetComponent<Transform>().position = pos;

        ////初期のカメラの回転処理
        //MovePlayerAngle();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 追跡対象がいなければ処理を行わない
        if (!m_originObject) return;

        if (!m_beforeOriginObject)
        {
            m_beforeOriginObject = true;

            m_lockOn = false;//自動的にロックオンしないようにする
            pos = GetComponent<Transform>().position;
            originRot = m_cameraObject.transform.rotation;//カメラの初期回転を取得

            m_rad = m_deg * Mathf.PI / 180;//ディグリーをラジアンにする

            //中心点の座標取得して自分もその座標にする
            originPos = m_originObject.transform.position;
            originPos.y = m_height;
            transform.position = originPos;


            //中心点からrangeの数値分距離を離す
            pos.x = originPos.x + (Mathf.Cos(m_rad) * m_range);
            pos.z = originPos.z + (Mathf.Sin(m_rad) * m_range);
            pos.y = m_height;

            //変更した数値をカメラに適応させる
            m_cameraObject.transform.position = pos;

            //初期のカメラの回転処理
            MovePlayerAngle();
        }

        //中心点の座標取得して自分もその座標にする
        originPos = m_originObject.transform.position;
        originPos.y = m_height;
        transform.position = originPos;

        //デルタタイム取得
        float delta = Time.deltaTime;
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
        m_rad += angle * delta;
        m_radX += angleY * delta;
        //m_angleX += angleY * delta;
        //var cameraRot = transform.eulerAngles;
        //cameraRot += new Vector3(angleY, angle) * delta;

        //// カメラ回転制限
        //if (-20.0f > m_angleX)
        //{
        //    cameraRot.x = -20.0f;
        //    m_angleX = -20.0f;
        //}
        //else if (20.0f < m_angleX)
        //{
        //    cameraRot.x = 20.0f;
        //    m_angleX = 20.0f;
        //}
        //transform.eulerAngles = cameraRot;//回転させる

        // レイ関係
        var cameraPos = m_cameraObject.GetComponent<Transform>().position;
        cameraPos.x = originPos.x + (Mathf.Cos(m_rad) * m_range);
        cameraPos.z = originPos.z + (Mathf.Sin(m_rad) * m_range);
        cameraPos.y = m_height;
        //cameraPos = transform.position + transform.rotation *
        //        Vector3.Scale(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, -10));

        var rayDirectionVec = cameraPos - originPos;
        float rayDistance = rayDirectionVec.magnitude;
        //Debug.Log("カメラの位置："+cameraPos);

        // レイを出す
        Debug.DrawRay(originPos, rayDirectionVec, new Color(0.0f, 1.0f, 0.0f, 1.0f));
        Ray ray = new Ray(originPos, rayDirectionVec);

        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, rayDistance))
        {
            var hitObj = rayHit.collider.gameObject;
            if (hitObj.tag == "Obstacle")
            {
                cameraPos = rayHit.point;
                m_cameraObject.GetComponent<Transform>().position = cameraPos;
            }
        }
        else
        {
            //Vector3 pushPos = new Vector3(2.0f, -3.0f, -2.0f);
            Vector3 pushPos = new Vector3(0.0f, 0.0f, -10.0f);

            //// 子オブジェクトとして追従しているような動きにする
            //pos = transform.position + transform.rotation *
            //    Vector3.Scale(transform.lossyScale, pushPos);

            pos.x = originPos.x + (Mathf.Cos(m_rad) * m_range);
            pos.z = originPos.z + (Mathf.Sin(m_rad) * m_range);
            pos.y = m_height;


            //中心点からrangeの数値分距離を離す
            m_cameraObject.GetComponent<Transform>().position = pos;
            m_cameraObject.transform.LookAt(m_originObject.transform.position);
        }

        //スティックの押し込みでPlayerの向いている方向に回り込む
        MovePlayerAngle();

        //LockOn(targetObject, false);

        //m_lockOn = false;//ロックオンしないようにする
        //m_cameraObject.transform.LookAt(originPos - new Vector3(0.0f, 2.0f, 0.0f));



        //float playerRad = m_originObject.GetComponent<Move>().GetAngle();
        //Debug.Log("プレイヤーの向いている方向" + (playerRad * 180 / Mathf.PI));
    }

    //回り込んでいる角度を渡す
    public float GetAngle()
    {
        Vector3 Vector = m_originObject.transform.position - m_cameraObject.transform.position;//方向ベクトルを取得する
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
}
