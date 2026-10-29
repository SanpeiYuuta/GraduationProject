using TMPro;
using UnityEngine;
/// <summary>
/// プレイヤーのHPを表示するUI
/// </summary>
public class UIPlayerHP : MonoBehaviour
{
    private GameObject m_player; // プレイヤー

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーが設定されていない場合は処理を行わない
        if (!m_player) return;

        var playerAvatar = m_player.GetComponent<PlayerAvatar>();

        var text = GetComponent<TextMeshProUGUI>();
        // プレイヤーのHPを表示する
        text.text = $"HP: {playerAvatar.HP}";
    }

    // プレイヤーを設定するセッタ
    public void SetPlayer(GameObject player)
    {
        // 既にプレイヤーが設定されている場合は、再設定しない
        if (m_player) return;

        m_player = player;
    }
}
