using Fusion;
using UnityEngine;

public class PlayerDamege : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("プレイヤーのトリガーに入ったよ");
        // トリガーに入ったオブジェクトがプレイヤーかどうかを判定
        if (other.gameObject.tag == "DamageObj")
        {
            // プレイヤーにダメージを与える処理をここに追加
            var player = GetComponent<PlayerAvatar>();
            player.HP -= 1; // HPを1減らす
            Debug.Log(player.HP);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("プレイヤーのトリガーから出たよ");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("プレイヤーはトリガーに当たり続けているよ");
    }


}
