using Fusion;
using UnityEngine;

public enum AttackType
{
    Melee, // 近接攻撃
    Ranged // 遠距離攻撃
}

//// 攻撃情報を相手に渡すための構造体
//public struct HitInfo
//{
//    // 攻撃の種類(遠距離か近距離か的な)
//    public AttackType attackType;

//    // 誰が攻撃したか(攻撃した瞬間は同期されてなければならないためNetWork限定)
//    public NetworkObject attacker; // 攻撃者のゲームオブジェクト

//    // 喰らった際のスタン時間
//    public float stunTime; // スタン時間

//    // ヒットストップ時間
//    public float hitStopTime; // ヒットストップ時間

//    public int attackPower; // 攻撃力
//    public float knockbackForce; // ノックバックの強さ
//    public Vector3 knockbackDirection; // ノックバックの方向

//    // 鳴らすSEの種類(後々追加するかも)
//}

public class DamageObject : MonoBehaviour
{
    // 攻撃情報
    [SerializeField] AttackDeta m_hitInfo;

    // 攻撃持続時間
    [SerializeField] float m_activeTime;

    private float m_delta = 0.0f;

    // 誰が攻撃したか(攻撃した瞬間は同期されてなければならないためNetWork限定)
    private NetworkObject m_parentObj;

    void Start()
    {

    }

    void Update()
    {
        m_delta = Time.deltaTime;

        m_activeTime -= m_delta;

        // 攻撃持続時間が0以下になった際自分は消える
        if (m_activeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("衝突したよ");

        //// 衝突したオブジェクトがプレイヤーかどうかを判定
        //if (collision.gameObject.tag == "Player")
        //{
        //    // プレイヤーにダメージを与える処理をここに追加
        //    var player = collision.gameObject.GetComponent<PlayerAvatar>();
        //    player.HP -= 1; // HPを1減らす

        //    //Debug.Log("Player hit! Apply damage.");
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        // 攻撃が当たったのが攻撃をしたプレイヤーならダメージ処理は無視する
        if(other.gameObject.TryGetComponent<NetworkObject>(out var networkObject))
        {
            Debug.Log("無視したよ");
            if (networkObject == m_parentObj) return;
        }

        Debug.Log("トリガーに入ったよ");
        // トリガーに入ったオブジェクトがプレイヤーかどうかを判定
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("ダメージを与えたい");
            // プレイヤーにダメージを与える処理をここに追加
            var player = other.gameObject.GetComponent<PlayerAvatar>();
            player.DamegeRpc(m_hitInfo.attackName);
            //player.HP -= 1; // HPを1減らす
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("トリガーから出たよ");
    }

    public float ActiveTime
    {
        get { return m_activeTime; }
        set { m_activeTime = value; }
    }

    public AttackDeta HitInfo
    {
        get { return m_hitInfo; }
    }

    // 親オブジェクトのセッタ
    public void SetParentObj(NetworkObject parentObj)
    {
        m_parentObj = parentObj;
    }
}