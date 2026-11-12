using Fusion;
using UnityEngine;

public abstract class PlayerStateAttackBase : PlayerStateBase
{
    // 攻撃マネージャー
    protected AttackManager m_attackManager;

    // 攻撃時間
    protected float m_attackTimeMax = 0.8f; // 全体の攻撃時間
    protected float m_attackTimeMaxFirst = 0.3f; // 一撃目を放つ攻撃時間

    // 攻撃発生時間
    protected float m_startAttackTime = 0.2f;
    // 攻撃時間計測
    protected float m_countAttackTime = 0.0f;

    // 次の攻撃ボタン受付時間
    protected float m_graceTimeNextAttack = 0.4f;
    // 次の攻撃に移行するかのフラグ
    protected bool m_isNextAttack = false;

    // プラスする攻撃力
    protected float m_addAttackPower = 0.0f;

    // 攻撃判定出現フラグ
    protected bool m_attackCollisionFlag = false;


    public PlayerStateAttackBase(NetworkObject parentObject, GameObject cameraObject) : base(parentObject, cameraObject)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        // 攻撃マネージャーを取得
        m_attackManager = GameObject.Find("AttackManager").GetComponent<AttackManager>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    // 攻撃コリジョン発生
    protected abstract void AttackCollisionOccurs();

    // 次の攻撃発生フラグ処理
    protected virtual void NextAttackFlag(bool onOff)
    {
        // フラグがオフだと通らない
        if (!onOff) return;

        // 攻撃ステートに移行
        if (m_playerInputDetas["Attack"].pressed)
        {
            m_isNextAttack = true;
        }
    }

    // 次のステート移動処理
    protected abstract void NextState();
}
