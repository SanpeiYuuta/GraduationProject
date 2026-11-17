using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.Unicode;
/// <summary>
/// 攻撃ステートの中で、魔弾を発射するステート
/// </summary>
public class StateShot : PlayerStateAttackBase
{
    // コンストラクタ
    public StateShot(NetworkObject parentObject, GameObject cameraObject) : base(parentObject,cameraObject)
    {
        m_attackTimeMax = 0.5f; // 全体の攻撃時間
        m_attackTimeMaxFirst = 0.2f; // 一撃目を放つ攻撃時間
        m_startAttackTime = 0.3f; // 攻撃発生時間
        //m_graceTimeNextAttack = 0.5f; // 次の攻撃ボタン受付時間 ※遠距離だからいらないかも
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
        // 次のステートに移行する
        NextState();

        // 移動反映
        m_playerAvatar.MoveVec = default;
        // 入力初期化
        m_playerAvatar.ResetInputDetas();


        //// 魔弾を発射する処理をここに書く
        ////if (m_attackAction.IsPressed())
        ////{
        ////    // 流石に冗長過ぎるので、後々攻撃判定用のオブジェクトを生成するスクリプトを作る予定
        ////    var bullet = m_parentObject.GetComponent<PlayerAvatar>().Runner.Spawn
        ////        (m_bullet, m_parentObject.transform.position + m_parentObject.transform.forward * 1.0f + new Vector3(0.0f, 1.0f, 0.0f), m_parentObject.transform.rotation);

        ////    // 親オブジェクトを魔弾に渡す
        ////    bullet.GetComponent<MagicBullet>().SetParentObj(m_parentObject);
        ////}

        //if(m_attackAction.IsPressed())
        //{
        //    m_currentAttack = true;
        //}
        //else
        //{
        //    m_currentAttack = false;
        //}

        //if (m_currentAttack != m_beforeAttack)
        //{
        //    // 攻撃
        //    if (m_currentAttack)
        //    {
        //        var attackManager = GameObject.Find("AttackManager").GetComponent<AttackManager>();
        //        Vector3 spawnPos = m_parentObject.transform.position + m_parentObject.transform.forward * 1.0f + new Vector3(0.0f, 1.0f, 0.0f);
        //        attackManager.CreateBulletObjectNetwork("MagicBullet",m_parentObject, spawnPos);

        //        //var runner = m_parentObject.GetComponent<PlayerAvatar>().Runner;

        //        //// 流石に冗長過ぎるので、後々攻撃判定用のオブジェクトを生成するスクリプトを作る予定
        //        //var bullet = m_parentObject.GetComponent<PlayerAvatar>().Runner.Spawn
        //        //    (m_bullet, m_parentObject.transform.position + m_parentObject.transform.forward * 1.0f + new Vector3(0.0f, 1.0f, 0.0f));

        //        //// 親オブジェクトを魔弾に渡す
        //        //bullet.GetComponent<MagicBullet>().SetParentObj(m_parentObject);
        //    }
        //    else if (!m_currentAttack)
        //    {
        //        m_currentAttack = false;
        //    }

        //    m_beforeAttack = m_currentAttack; // フラグを更新
        //}
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
            var attackManager = GameObject.Find("AttackManager").GetComponent<AttackManager>();
            Vector3 spawnPos = m_parentObject.transform.position + m_parentObject.transform.forward * 1.0f + new Vector3(0.0f, 1.0f, 0.0f);
            attackManager.CreateBulletObjectNetwork("MagicBullet", m_parentObject, spawnPos);

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
