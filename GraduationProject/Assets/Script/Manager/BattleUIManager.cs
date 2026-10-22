using TMPro;
using UnityEngine;
/// <summary>
/// バトル中のUIを管理するクラス
/// </summary>
public class BattleUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_battleStartUI; // バトル中のUIのオブジェクト
    [SerializeField] TextMeshProUGUI m_battlePreparation; // バトル準備中のUIのオブジェクト

    private E_GamePhase m_gamePhase = E_GamePhase.None; // ゲームのフェーズ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームのフェーズを取得する
        var gameManager = GameObject.Find("GameBattleManager(Clone)").GetComponent<GameBattleManager>();

        if (!gameManager)
        {
            Debug.Log("とおらない");
        }
        m_gamePhase = gameManager.GetGamePhase();
        //Debug.Log($"ゲームフェーズは{m_gamePhase}です。");

        // ゲームのフェーズによってUIを切り替える
        switch (m_gamePhase)
        {
            case E_GamePhase.Preparation:
                Debug.Log("バトル準備中のUIを表示");
                m_battlePreparation.gameObject.SetActive(true);
                m_battleStartUI.gameObject.SetActive(false);
                break;
            case E_GamePhase.Start:
                m_battlePreparation.gameObject.SetActive(false);
                m_battleStartUI.gameObject.SetActive(true);
                break;
            case E_GamePhase.Battle:
                m_battlePreparation.gameObject.SetActive(false);
                m_battleStartUI.gameObject.SetActive(false);
                break;
            case E_GamePhase.Result:
                m_battlePreparation.gameObject.SetActive(false);
                m_battleStartUI.gameObject.SetActive(false);
                break;
            case E_GamePhase.End:
                m_battlePreparation.gameObject.SetActive(false);
                m_battleStartUI.gameObject.SetActive(false);
                break;
        }
    }
}
