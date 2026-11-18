using TMPro;
using UnityEngine;
/// <summary>
/// サンドバック
/// ダメージどれぐらい渡せたか、ノックバックどんな感じに動くか確認するためのオブジェクト
/// </summary>

public class PunchingBag : MonoBehaviour
{
    private float m_totalDamage; // トータルの攻撃量
    private float m_currentdamage; // 現在受けたダメージ

    private GameObject m_totalDamageText;
    private GameObject m_currentDamageText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 子オブジェクト取得
        m_totalDamageText = transform.Find("TotalDamageText").gameObject;
        m_currentDamageText = transform.Find("CurrentDamageText").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // ダメージ量をテキストに表示する
        m_currentDamageText.GetComponent<TextMeshPro>().text = m_currentdamage.ToString();
        m_totalDamageText.GetComponent<TextMeshPro>().text = m_totalDamage.ToString();
    }

    // ダメージ処理
    public void Damage(string attackName)
    {
        // 攻撃マネージャー取得 このエラー確認方法はよろしくないので修正する予定
        var attackManagerObj = GameObject.Find("AttackManager");
        if (!attackManagerObj) return;
        var attackManager = attackManagerObj.GetComponent<AttackManager>();

        // 攻撃データの取得
        var attackDeta = attackManager.GetAttackDeta(attackName);

        // ダメージ処理
        m_currentdamage = attackDeta.attackPower;
        m_totalDamage += m_currentdamage;
    }
}
