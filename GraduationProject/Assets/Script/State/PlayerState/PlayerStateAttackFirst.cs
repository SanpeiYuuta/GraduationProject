using Fusion;
using UnityEngine;
/// <summary>
/// 攻撃の一段目のステート(近接)
/// </summary>

public class PlayerStateAttackFirst : PlayerStateAttackBase
{
    public PlayerStateAttackFirst(NetworkObject parentObject, GameObject cameraObject) : base(parentObject, cameraObject)
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
        // 攻撃を出す
        if (m_countAttackTime >= m_attackTimeMaxFirst && !m_attackCollisionFlag)
        {
            // プレイヤーの前方に攻撃オブジェクトを生成
            Vector3 createPos = m_parentObject.transform.position + m_parentObject.transform.forward * 1.0f;
            var damegeObj = m_attackManager.CreateAttackObjectNetwork("PlayerAttackFirst", m_parentObject, createPos);

            m_attackCollisionFlag = true;
        }
    }

    // ステート移行処理
    protected override void NextState()
    {
        var attackAction = m_playerInputDetas["Attack"];
        var dodgeAction = m_playerInputDetas["Dodge"];
        if (attackAction.pressed) m_isNextAttack = true;

        //次のステートに遷移する
        if (m_isNextAttack && m_countAttackTime >= m_graceTimeNextAttack)
        {
            m_playerAvatar.GetStateMachinePlayer().ChangeState("AttackSecond");
        }
        else if (dodgeAction.pressed && m_countAttackTime >= m_graceTimeNextAttack) // 回避状態移行
        {
            if (m_playerAvatar.CanDodgeFlag) return;

            m_playerAvatar.GetStateMachinePlayer().ChangeState("Dodge");
            m_playerAvatar.CanDodgeFlag = false;
        }
        else if (m_countAttackTime >= m_attackTimeMax)
        {
            // 攻撃が一定時間過ぎたら元のステートに戻る
            m_playerAvatar.GetStateMachinePlayer().ChangeState("Walk");
        }
    }
}
