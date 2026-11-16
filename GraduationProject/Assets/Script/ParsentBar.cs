using Fusion;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// HPバーやMPバーなどをバー表示するためのクラス
/// </summary>
public class ParsentBar : MonoBehaviour
{
    private NetworkObject m_player; // プレイヤー
    private BattleUIManager m_uiManager; // UIマネージャー

    private Color m_normalColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_normalColor = GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        m_uiManager = GameObject.Find("BattleUIManager").GetComponent<BattleUIManager>();
        m_player = m_uiManager.PlayerObj;

        if (!m_player) return;

        var max = m_player.GetComponent<PlayerAvatar>().MaxBarrierEnergy;
        var current = m_player.GetComponent<PlayerAvatar>().CurrentBarrierEnergy;

        // current0未満対策
        if (current < 0) current = 0.0f;

        var pasent = current / max;

        float dangerZone = 0.2f;

        // 残量が危険域になったら赤くする
        if (pasent <= dangerZone)
        {
            GetComponent<Image>().color = Color.red;
        }
        else
        {
            // 元の色に戻す
            if (m_normalColor != GetComponent<Image>().color)
            {
                GetComponent<Image>().color = m_normalColor;
            }
        }

        // ゲージ反映
        GetComponent<Image>().fillAmount = pasent;
    }
}
