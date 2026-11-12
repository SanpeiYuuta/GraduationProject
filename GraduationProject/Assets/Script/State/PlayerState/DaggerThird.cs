using Fusion;
using UnityEngine;

public class DaggerThird : PlayerStateAttackBase
{
    public DaggerThird(NetworkObject parentObject, GameObject cameraObject) : base(parentObject, cameraObject)
    {
        m_attackTimeMax = 0.6f; // 全体の攻撃時間
        m_attackTimeMaxFirst = 0.2f; // 一撃目を放つ攻撃時間
        m_startAttackTime = 0.2f; // 攻撃発生時間
        m_graceTimeNextAttack = 0.4f; // 次の攻撃ボタン受付時間
    }

    public override void OnEnter()
    {
        Debug.Log("三段目攻撃");
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
        NextState();
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
            var damegeObj = m_attackManager.CreateAttackObjectNetwork("DaggerAttackThird", m_parentObject, createPos);
            m_attackCollisionFlag = true;
        }
    }

    // 次のステートに移行するかの判定
    protected override void NextState()
    {
        // 攻撃が一定時間過ぎたら元のステートに戻る
        if (m_countAttackTime >= m_attackTimeMax)
        {
            m_playerAvatar.GetStateMachinePlayer().ChangeState("Walk");
        }
    }

}
