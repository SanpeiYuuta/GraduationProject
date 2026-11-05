using Fusion;
using UnityEngine;
/// <summary>
/// 攻撃の一段目のステート(近接)
/// </summary>

public class PlayerStateAttackFirst : PlayerStateAttackBase
{
    // 攻撃判定出現フラグ
    bool m_attackCollisionFlag = false;

    public PlayerStateAttackFirst(NetworkObject parentObject, GameObject cameraObject) : base(parentObject, cameraObject)
    {
        m_attackTimeMax = 0.8f; // 全体の攻撃時間
        m_attackTimeMaxFirst = 0.3f; // 一撃目を放つ攻撃時間
        m_startAttackTime = 0.2f; // 攻撃発生時間
        //m_graceTimeNextAttack = 0.4f; // 次の攻撃ボタン受付時間
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // 初期化
        m_countAttackTime = 0;
        m_attackCollisionFlag = false;

        // ここでSEを鳴らす

        // アニメーションを再生する

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_countAttackTime += m_deltaTime;

        var attackManager = GameObject.Find("AttackManager").GetComponent<AttackManager>();

        // 攻撃を出す
        if(m_countAttackTime >= m_attackTimeMaxFirst && !m_attackCollisionFlag)
        {
            // プレイヤーの前方に攻撃オブジェクトを生成
            Vector3 createPos = m_parentObject.transform.position + m_parentObject.transform.forward * 1.0f; 
            var damegeObj = attackManager.CreateAttackObject("PlayerAttackFirst", createPos);
            damegeObj.GetComponent<DamageObject>().SetParentObj(m_parentObject);

            m_attackCollisionFlag = true;
        }

        // 攻撃が一定時間過ぎたら元のステートに戻る
        if(m_countAttackTime >= m_attackTimeMax)
        {
            m_parentObject.GetComponent<PlayerAvatar>().GetStateMachinePlayer().ChangeState("Walk");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}
