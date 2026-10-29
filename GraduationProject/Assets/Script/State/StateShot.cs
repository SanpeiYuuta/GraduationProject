using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.Unicode;
/// <summary>
/// 攻撃ステートの中で、魔弾を発射するステート
/// </summary>
public class StateShot : StateBase
{
    private InputAction m_attackAction;
    [SerializeField] GameObject m_bullet; // テスト用の魔弾のプレハブを入れておく

    private bool m_currentAttack = false; // 攻撃中かどうかのフラグ
    private bool m_beforeAttack = false; // 攻撃前のフラグ

    // コンストラクタ
    public StateShot(NetworkObject parentObject, GameObject bullet) : base(parentObject)
    {
        m_bullet = bullet;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        m_attackAction = InputSystem.actions.FindAction("Attack");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        // 魔弾を発射する処理をここに書く
        //if (m_attackAction.IsPressed())
        //{
        //    // 流石に冗長過ぎるので、後々攻撃判定用のオブジェクトを生成するスクリプトを作る予定
        //    var bullet = m_parentObject.GetComponent<PlayerAvatar>().Runner.Spawn
        //        (m_bullet, m_parentObject.transform.position + m_parentObject.transform.forward * 1.0f + new Vector3(0.0f, 1.0f, 0.0f), m_parentObject.transform.rotation);

        //    // 親オブジェクトを魔弾に渡す
        //    bullet.GetComponent<MagicBullet>().SetParentObj(m_parentObject);
        //}

        if(m_attackAction.IsPressed())
        {
            m_currentAttack = true;
        }
        else
        {
            m_currentAttack = false;
        }

        if (m_currentAttack != m_beforeAttack)
        {
            // 攻撃
            if (m_currentAttack)
            {
                var runner = m_parentObject.GetComponent<PlayerAvatar>().Runner;

                // 流石に冗長過ぎるので、後々攻撃判定用のオブジェクトを生成するスクリプトを作る予定
                var bullet = m_parentObject.GetComponent<PlayerAvatar>().Runner.Spawn
                    (m_bullet, m_parentObject.transform.position + m_parentObject.transform.forward * 1.0f + new Vector3(0.0f, 1.0f, 0.0f));

                // 親オブジェクトを魔弾に渡す
                bullet.GetComponent<MagicBullet>().SetParentObj(m_parentObject);
            }
            else if (!m_currentAttack)
            {
                m_currentAttack = false;
            }

            m_beforeAttack = m_currentAttack; // フラグを更新
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
