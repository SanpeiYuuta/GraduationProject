using Fusion;
using UnityEngine;
/// <summary>
/// ハンドガンの腰打ちステート
/// </summary>

public class HandgunHipShotState : PlayerStateAttackBase
{
    public HandgunHipShotState(NetworkObject parentObject,GameObject cameraObject):base(parentObject,cameraObject)
    {
        m_attackTimeMax = 0.5f; // 全体の攻撃時間
        m_attackTimeMaxFirst = 0.2f; // 一撃目を放つ攻撃時間
        m_startAttackTime = 0.1f; // 攻撃発生時間
        m_graceTimeNextAttack = 0.25f; // 次の攻撃ボタン受付時間
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // 初期化
        m_countAttackTime = 0;
        m_attackCollisionFlag = false;
        m_isNextAttack = false;

        // ここでSEを鳴らす

        // アニメーションを再生する
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_countAttackTime += m_deltaTime;

        // 攻撃を出す
        AttackCollisionOccurs();

        // 次の攻撃に移行してよいか確認をする
        NextAttackFlag(true);
        NextState();

        // 移動反映(移動させない)
        m_playerAvatar.MoveVec = default;
        // 入力初期化
        m_playerAvatar.ResetInputDetas();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    // 攻撃コリジョン発生
    protected override void AttackCollisionOccurs()
    {
        var playerPos = m_playerAvatar.transform.position;
        var playerForward = m_playerAvatar.transform.forward;

        // 攻撃(攻撃用レイ)を出す
        if (m_countAttackTime >= m_attackTimeMaxFirst && !m_attackCollisionFlag)
        {
            // プレイヤーの前方に攻撃用レイを出す
            Debug.DrawRay(playerPos, playerForward * 10.0f, new Color(1.0f, 0.0f, 0.0f));
            Ray attackRay = new Ray(playerPos, playerForward);
            float lenght = 10.0f;

            RaycastHit hit;
            if(Physics.Raycast(attackRay,out hit,lenght))
            {
                var hitObj = hit.collider.gameObject;
                if(hitObj.tag == "PunchingBag")
                {
                    var punchingBag = hitObj.GetComponent<PunchingBag>();
                    punchingBag.Damage("HandgunBullet");
                }
            }

            m_attackCollisionFlag = true;
        }
    }

    // ステート移行処理
    protected override void NextState()
    {
        if (m_countAttackTime >= m_attackTimeMax)
        {
            // 攻撃が一定時間過ぎたら元のステートに戻る
            m_playerAvatar.GetStateMachinePlayer().ChangeState("Walk");
        }
    }
}
