using Fusion;
using UnityEngine;
/// <summary>
/// 攻撃の二段目のステート(近接)
/// </summary>

public class PlayerStateAttackSecond : PlayerStateAttackBase
{
    public PlayerStateAttackSecond(NetworkObject parentObject,GameObject cameraObject) : base(parentObject, cameraObject)
    {
        m_attackTimeMax = 0.8f; // 全体の攻撃時間
        m_attackTimeMaxFirst = 0.3f; // 一撃目を放つ攻撃時間
        m_startAttackTime = 0.2f; // 攻撃発生時間
        m_graceTimeNextAttack = 0.4f; // 次の攻撃ボタン受付時間
    }

    public override void OnEnter()
    {
        Debug.Log("二段目攻撃");

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
        
        // 次のステート移行処理
        NextAttackFlag(true);
        NextState();

        // 移動反映(移動させない)
        m_parentObject.GetComponent<PlayerAvatar>().MoveVec = default;
        // 入力初期化
        m_parentObject.GetComponent<PlayerAvatar>().ResetInputDetas();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected override void AttackCollisionOccurs()
    {
        // 攻撃を出す
        if (m_countAttackTime >= m_attackTimeMaxFirst && !m_attackCollisionFlag)
        {
            // プレイヤーの前方に攻撃オブジェクトを生成
            Vector3 createPos = m_parentObject.transform.position + m_parentObject.transform.forward * 1.0f;
            var damegeObj = m_attackManager.CreateAttackObjectNetwork("PlayerAttackSecond", m_parentObject, createPos);

            m_attackCollisionFlag = true;
        }
    }

    protected override void NextState()
    {
        var attackAction = m_playerInputDetas["Attack"];
        var dodgeAction = m_playerInputDetas["Dodge"];
        if (attackAction.pressed) m_isNextAttack = true;

        //次のステートに遷移する
        if (m_isNextAttack && m_countAttackTime >= m_graceTimeNextAttack)
        {
            m_parentObject.GetComponent<PlayerAvatar>().GetStateMachinePlayer().ChangeState("AttackThird");
        }
        else if (dodgeAction.pressed && m_countAttackTime >= m_graceTimeNextAttack) // 回避状態移行
        {
            if (!m_playerAvatar.CanDodgeFlag) return;

            m_parentObject.GetComponent<PlayerAvatar>().GetStateMachinePlayer().ChangeState("Dodge");
            m_playerAvatar.CanDodgeFlag = false;
        }
        else if (m_countAttackTime >= m_attackTimeMax)
        {
            // 攻撃が一定時間過ぎたら元のステートに戻る
            m_parentObject.GetComponent<PlayerAvatar>().GetStateMachinePlayer().ChangeState("Walk");
        }

    }

}
