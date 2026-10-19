using UnityEngine;

public class DamageObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("衝突したよ");

    //    // 衝突したオブジェクトがプレイヤーかどうかを判定
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        // プレイヤーにダメージを与える処理をここに追加
    //        var player = collision.gameObject.GetComponent<PlayerAvatar>();
    //        player.HP -= 1; // HPを1減らす

    //        //Debug.Log("Player hit! Apply damage.");
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("トリガーに入ったよ");
        // トリガーに入ったオブジェクトがプレイヤーかどうかを判定
        if (other.gameObject.tag == "Player")
        {
            // プレイヤーにダメージを与える処理をここに追加
            var player = other.gameObject.GetComponent<PlayerAvatar>();
            //player.HP -= 1; // HPを1減らす
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("トリガーから出たよ");
    }
}
