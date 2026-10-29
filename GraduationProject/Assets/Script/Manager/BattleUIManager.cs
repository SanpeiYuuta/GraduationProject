using TMPro;
using UnityEngine;
/// <summary>
/// バトル中のUIを管理するクラス
/// </summary>
public class BattleUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_battleStartUI; // バトル中のUIのオブジェクト
    [SerializeField] TextMeshProUGUI m_battlePreparation; // バトル準備中のUIのオブジェクト
    [SerializeField] TextMeshProUGUI m_battleGameSetUI; // バトル準備中のUIのオブジェクト
    [SerializeField] TextMeshProUGUI m_battleWinerUI; // 勝者が分かるUI
    [SerializeField] GameObject m_reStartButton; // リスタート用のボタン

    private E_GamePhase m_gamePhase = E_GamePhase.None; // ゲームのフェーズ
    private E_GamePhase m_beforGamePhase = E_GamePhase.None; // 前フレームのゲームのフェーズ

    private GameBattleManager m_gameBattleManager = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームのフェーズを取得する
        m_gameBattleManager = GameObject.Find("GameBattleManager(Clone)").GetComponent<GameBattleManager>();

        if (!m_gameBattleManager)
        {
            Debug.Log("とおらない");
            return;
        }
        m_gamePhase = m_gameBattleManager.GetGamePhase();
        //Debug.Log($"ゲームフェーズは{m_gamePhase}です。");

        // フェーズの切り替わりを見る
        if(m_beforGamePhase != m_gamePhase)
        {
            // ゲームのフェーズによってUIを切り替える
            UIActiveChange();
        }


    }

    // フェーズ切り替えによるUIの表示変更
    private void UIActiveChange()
    {
        switch (m_gamePhase)
        {
            case E_GamePhase.Preparation:
                m_battlePreparation.gameObject.SetActive(true);

                m_battleStartUI.gameObject.SetActive(false);
                m_battleGameSetUI.gameObject.SetActive(false);
                m_battleWinerUI.gameObject.SetActive(false);
                m_reStartButton.gameObject.SetActive(false);
                break;
            case E_GamePhase.Start:
                m_battleStartUI.gameObject.SetActive(true);

                m_battlePreparation.gameObject.SetActive(false);
                m_battleGameSetUI.gameObject.SetActive(false);
                m_battleWinerUI.gameObject.SetActive(false);
                m_reStartButton.gameObject.SetActive(false);
                break;
            case E_GamePhase.Battle:
                m_battlePreparation.gameObject.SetActive(false);
                m_battleStartUI.gameObject.SetActive(false);
                m_battleGameSetUI.gameObject.SetActive(false);
                m_battleWinerUI.gameObject.SetActive(false);
                m_reStartButton.gameObject.SetActive(false);
                break;
            case E_GamePhase.BattleEnd:
                m_battleGameSetUI.gameObject.SetActive(true);

                m_battlePreparation.gameObject.SetActive(false);
                m_battleStartUI.gameObject.SetActive(false);
                m_battleWinerUI.gameObject.SetActive(false);
                m_reStartButton.gameObject.SetActive(false);
                break;
            case E_GamePhase.Result:
                // 使わないUIを出さない
                m_battlePreparation.gameObject.SetActive(false);
                m_battleStartUI.gameObject.SetActive(false);
                m_battleGameSetUI.gameObject.SetActive(false);
                m_reStartButton.gameObject.SetActive(false);
                m_battleWinerUI.gameObject.SetActive(false); // これはフラグがオンでないと使わないから

                // 決着が付いたか確認する
                var battlePlayers = m_gameBattleManager.GetBattlePlayers();
                bool flag = false;
                foreach (var player in battlePlayers)
                {
                    int playerHp = player.GetComponent<PlayerAvatar>().HP; // プレイヤーのHPをチェックする

                    if (playerHp <= 0)
                    {
                        flag = true;
                        continue;
                    }
                }

                // 決着が着かない限り勝者をUIで表示できない
                if (!flag) return;

                // 勝者が誰か探す
                int playerId = 0;
                foreach (var player in battlePlayers)
                {
                    int playerHp = player.GetComponent<PlayerAvatar>().HP; // プレイヤーのHPをチェックする
                    if (playerHp > 0)
                    {
                        flag = true;
                        playerId = player.GetComponent<PlayerAvatar>().Id;
                        continue;
                    }
                }


                m_battleWinerUI.gameObject.SetActive(true);
                m_battleWinerUI.text = $"Winer : {playerId}P"; // 勝者表示


                break;
            case E_GamePhase.End:
                m_battleWinerUI.gameObject.SetActive(true);
                m_reStartButton.gameObject.SetActive(true);

                m_battlePreparation.gameObject.SetActive(false);
                m_battleStartUI.gameObject.SetActive(false);
                m_battleGameSetUI.gameObject.SetActive(false);
                break;
        }

    }
}
