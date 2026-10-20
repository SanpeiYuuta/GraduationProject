using UnityEngine;
/// <summary>
/// プレイヤーの攻撃シリーズ　魔法(魔弾をイメージ)
/// </summary>

public class PlayerAttack_Magic : MonoBehaviour
{
    // 攻撃フラグ
    private bool m_currentIsAttack = false;
    private bool m_beforeIsAttack = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_beforeIsAttack != m_currentIsAttack)
        {
            if (m_currentIsAttack)
            {
                // 攻撃開始時の処理
                Debug.Log("攻撃開始");
            }
            else if(!m_currentIsAttack)
            {
                // 攻撃終了時の処理
                Debug.Log("攻撃終了");
            }
            m_beforeIsAttack = m_currentIsAttack;
        }




    }
}
