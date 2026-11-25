using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

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

        var avater = m_parentObject.GetComponent<PlayerAvatar>();
        var inputDeta = avater.GetInputDetas();

        var dodgeInputDeta = inputDeta["Dodge"];
        var attackAction = inputDeta["Attack"];

        // ステート移行
        if (dodgeInputDeta.pressed && avater.CanDodgeFlag) // 回避状態移行
        {
            var myState = m_playerAvatar.GetStateMachinePlayer();
            myState.ChangeState("Dodge");
            m_playerAvatar.CanDodgeFlag = false;
        }
        else if (attackAction.pressed) // 攻撃状態移行
        {
            AttackStateChange();
            //var myState = m_playerAvatar.GetStateMachinePlayer();
            //myState.ChangeState("AttackFirst");
        }


        var dashAction = m_playerInputActions["Dash"];

        // 移動処理
        Vector3 move = new Vector3(0.0f, 0.0f, 0.0f);
        Quaternion direction = new Quaternion(1.0f,1.0f,1.0f,1.0f);
        var controllerVec = m_playerInputActions["Move"].ReadValue<Vector2>();
        //Debug.Log("m_directionVec: " + m_directionVec);

        // カメラの向いている方向を取得
        var cameraQt = m_camera.transform.rotation;
        var cameraRot = m_camera.transform.eulerAngles;

        // カメラ追従フラグ取得
        bool camerafllow = m_playerAvatar.CameraFllow;

        switch (camerafllow)
        {
            case false:
                if (dashAction.IsPressed() && controllerVec != new Vector2(0.0f, 0.0f))
                {
                    // ダッシュ用のスピードの調整したのち移動更新
                    move = m_directionVec * (m_speed * 3.0f);
                    
                    direction = Quaternion.Euler(0.0f, (m_totalAngle * Mathf.Rad2Deg) + 90.0f, 0.0f);
                    m_playerAvatar.Rotation = direction;
                }
                else if (controllerVec != new Vector2(0.0f, 0.0f))
                {
                    // 徒歩用のスピードの調整したのち移動更新
                    move = m_directionVec * m_speed;

                    direction = Quaternion.Euler(0.0f, (m_totalAngle * Mathf.Rad2Deg) + 90.0f, 0.0f);
                    m_playerAvatar.Rotation = direction;
                }
                break;
            case true:
                if (controllerVec != new Vector2(0.0f, 0.0f))
                {
                    float totalSpeed = 0.0f;
                    // 反対方向に向く動きは遅くする
                    var dot = Vector3.Dot(m_directionVec, m_playerAvatar.transform.forward);
                    float DecelerationRatio = 0.65f;
                    totalSpeed = Mathf.Lerp(m_speed, m_speed * DecelerationRatio, -dot);
                    Debug.Log(totalSpeed);
                    //}

                    // 徒歩用のスピードの調整したのち移動更新
                    move = m_directionVec * totalSpeed;

                    direction = Quaternion.Euler(0.0f, cameraRot.y, 0.0f);
                    m_playerAvatar.Rotation = direction;
                }
                break;
        }


        //if (dashAction.IsPressed() && controllerVec != new Vector2(0.0f,0.0f))
        //{
        //    // ダッシュ用のスピードの調整したのち移動更新
        //    move = m_directionVec * (m_speed * 3.0f);
        //}
        //else if(controllerVec != new Vector2(0.0f, 0.0f))
        //{
        //    // 徒歩用のスピードの調整したのち移動更新
        //    move = m_directionVec * m_speed;
        //}

        //// プレイヤーの回転処理
        //switch(camerafllow)
        //{
        //    case false:
        //        direction = Quaternion.Euler(0.0f, (m_totalAngle * Mathf.Rad2Deg) + 90.0f, 0.0f);
        //        m_playerAvatar.Rotation = direction;
        //        break;
        //    case true:
        //        direction = Quaternion.Euler(0.0f, cameraRot.y, 0.0f);
        //        m_playerAvatar.Rotation = direction;
        //        break;
        //}

        // 移動反映
        avater.MoveVec = move;
        // 入力初期化
        avater.ResetInputDetas();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    // 攻撃ステートに遷移する際の関数
    private void AttackStateChange()
    {
        var myState = m_playerAvatar.GetStateMachinePlayer();

        var loadout = m_playerAvatar.GetPlayerLoadout();
        var loadoutNum = loadout.m_currentWeaponNum;
        var currentWeapon = loadout.m_gearWeaponList[loadoutNum];

        switch (currentWeapon)
        {
            case E_weapon.Katana:
                myState.ChangeState("AttackFirst");
                break;
            case E_weapon.Dagger:
                myState.ChangeState("DaggerFirst");
                break;
            case E_weapon.Greatsword:
                myState.ChangeState("GreatswordFirst");
                break;
            case E_weapon.MagicBullet:
                myState.ChangeState("BulletAttack");
                break;
            case E_weapon.Handgun:
                myState.ChangeState("HandgunHipShot");
                break;
            default:
                break;
        }
    }
}
