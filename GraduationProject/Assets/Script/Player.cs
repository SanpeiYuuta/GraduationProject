using UnityEngine;
/// <summary>
/// プレイヤーの基盤処理(ネットワークで同期させなくても良い処理をここに入れておく)
/// </summary>

public class Player : MonoBehaviour
{
    private PlayerAvatar m_avatar;

    private float m_currentHp;
    private float m_beforHp;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_avatar = GetComponent<PlayerAvatar>();

        // HP更新
        m_currentHp = m_avatar.HP;

        if (m_currentHp == m_beforHp) return; 

        // hpが0以下になったら見えなくする
        if (m_currentHp <= 0)
        {
            m_avatar.Active = false;
            Transform model = transform.Find("Armature");
            model.gameObject.SetActive(false); // モデルを非表示にする(更新もしない)

            //gameObject.SetActive(false);
        }
        else if(m_currentHp > 0)
        {
            m_avatar.Active = true;
            Transform model = transform.Find("Armature");
            model.gameObject.SetActive(true); // モデルを非表示にする(更新もしない)
        }

        m_beforHp = m_currentHp;
    }
}
