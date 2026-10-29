using Fusion;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 魔弾の動きを制御するスクリプト
/// </summary>

public class MagicBullet : NetworkBehaviour
{
    private float m_speed = 12.0f; // 魔弾の速度

    private Vector3 m_startPos; // 生成されたときの位置
    private float m_moveDistance = 30.0f; // 魔弾の移動可能距離

    private Vector3 m_directionVec; // 魔弾の進行方向
    private Vector3 m_moveVec; // 魔弾の移動量

    private NetworkObject m_parentObj; // 魔弾を発射したオブジェクト
    private NetworkObject m_beforparentObj;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Spawned()
    {
        m_startPos = transform.position;

        // 球を発射したオブジェクトを受け取る
        //var player = m_parentObj.GetComponent<PlayerAvatar>();

        // 球を発射する方向を決める（プレイヤーの向いている方向にする）
        //m_directionVec = player.transform.forward;
        m_directionVec = new Vector3(0,0,1);
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        float delta = Runner.DeltaTime;

        if (m_parentObj != m_beforparentObj)
        {
            if (m_parentObj)
            {
                // 球を発射したオブジェクトを受け取る
                var player = m_parentObj.GetComponent<PlayerAvatar>();

                // 球を発射する方向を決める（プレイヤーの向いている方向にする）
                m_directionVec = player.transform.forward;
            }

            m_beforparentObj = m_parentObj;
        }

        // 親オブジェクトが設定されていない場合は処理を行わない
        if (!m_parentObj) return;

        Debug.Log("更新できている");
        // 移動距離が0以下になったら消滅する
        if (m_moveDistance <= 0)
        {
            Debug.Log("魔弾が消滅しました");
            //var player = m_parentObj.GetComponent<PlayerAvatar>();
            Runner.Despawn(Object);
            return;
            //NetworkRunner.Despawn();
        }

        // 移動
        m_moveVec = m_directionVec * m_speed * delta;

        // 移動反映
        transform.position += m_moveVec;
        m_moveDistance -= m_moveVec.magnitude;
    }

    public void SetParentObj(NetworkObject obj)
    {
        m_parentObj = obj;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("球がプレイヤーのトリガーに入ったよ");
        // トリガーに入ったオブジェクトがプレイヤーかどうかを判定
        if (other.gameObject.tag == "Player")
        {
            // プレイヤーにダメージを与える処理をここに追加
            var player = other.gameObject.GetComponent<PlayerAvatar>();
            //player.DamegeRpc(1);
            //player.m_hp -= 1; // HPを1減らす
            player.HP -= 1;
            //Debug.Log(player.HP);
        }
    }

}
