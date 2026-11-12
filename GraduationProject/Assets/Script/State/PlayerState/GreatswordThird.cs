using Fusion;
using UnityEngine;

public class GreatswordThird : PlayerStateAttackBase
{
    public GreatswordThird(NetworkObject parentObject, GameObject cameraObject) : base(parentObject, cameraObject)
    {
        m_attackTimeMax = 1.2f; // 全体の攻撃時間
        m_attackTimeMaxFirst = 0.6f; // 一撃目を放つ攻撃時間
        m_startAttackTime = 0.4f; // 攻撃発生時間
        m_graceTimeNextAttack = 0.6f; // 次の攻撃ボタン受付時間
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
            var damegeObj = m_attackManager.CreateAttackObjectNetwork("GreatswordAttackThird", m_parentObject, createPos);

            m_attackCollisionFlag = true;
        }
    }

    // ステート移行処理
    protected override void NextState()
    {
        // 攻撃が一定時間過ぎたら元のステートに戻る
        if (m_countAttackTime >= m_attackTimeMax)
        {
            m_playerAvatar.GetStateMachinePlayer().ChangeState("Walk");
        }
    }
}
