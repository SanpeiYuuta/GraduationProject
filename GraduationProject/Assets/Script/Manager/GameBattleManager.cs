using Fusion;
using NUnit.Framework;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using static Unity.Collections.Unicode;
/// <summary>
/// バトルシーンでのシステムを管理する
/// </summary>
/// 
public enum E_GamePhase
{
    None,
    Preparation, // ゲーム準備のフェーズ(ここで誰が戦闘をするのかどのチームにするか決める)
    Start, // ゲーム開始時のフェーズ(ここで開始演出とか)
    Battle, // 実際に戦闘を行うフェーズ
    BattleEnd, // 戦闘終了フェーズ
    Result, // 誰が勝ったかを表示するフェーズ
    End, // ゲーム終了時のフェーズ(ここで終了処理)
}

public class GameBattleManager : NetworkBehaviour
{

    [SerializeField] int m_maxPlayerNum = 2; // 最大プレイヤー数

    // プレイヤーの実体を保管する配列
    private List<NetworkObject> m_playerObjects = new List<NetworkObject>();

    // バトルするプレイヤーを保管する配列
    private List<NetworkObject> m_battlePlayers = new List<NetworkObject>();

    //private Vector<> m_players;

    private E_GamePhase m_gamePhase = E_GamePhase.None; // ゲームのフェーズ

    [SerializeField] float m_maxStartTextTime; // ゲーム開始テキストを出す最大時間
    private float m_currentStartTextTime; // ゲーム開始テキストを出している時間

    [SerializeField] float m_maxBattleEndTextTime; // ゲーム開始テキストを出す最大時間
    private float m_currentBattleEndTextTime; // ゲーム開始テキストを出している時間

    [SerializeField] float m_maxResultTextTime; // 誰が勝ったかのリザルトを出す時間
    private float m_currentResultTextTime; // 誰が勝ったかのリザルトを出す時間

    [SerializeField] TextMeshPro m_textMeshPro;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Spawned()
    {
        m_gamePhase = E_GamePhase.Preparation; // ゲームフェーズを準備にする
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        Debug.Log("現在のフェーズは" + m_gamePhase);

        // 戦闘人数に満たしていない時は処理しない
        if (m_playerObjects.Count < m_maxPlayerNum) return;

        float delta = Runner.DeltaTime;

        // 準備
        if (m_gamePhase == E_GamePhase.Preparation)
        {
            int count = 0;
            foreach (var player in m_playerObjects)
            {
                // 戦闘するプレイヤーを戦闘用の配列に入れる
                count++;
                // 戦闘人数が最大人数を超えていたら処理しない
                if (count > m_maxPlayerNum) continue;
                m_battlePlayers.Add(player);
            }

            Debug.Log("ゲームが開始されます");
            m_gamePhase = E_GamePhase.Start; // ゲームフェーズを開始にする
            return;
        }

        // 開始
        if (m_gamePhase == E_GamePhase.Start)
        {
            // 開始演出をする(後々演出を入れるために書いておく)
            m_currentStartTextTime += delta;

            if (m_currentStartTextTime < m_maxStartTextTime) return;

            m_gamePhase = E_GamePhase.Battle; // ゲームフェーズを戦闘にする
            return;
        }


        // 戦闘
        if (m_gamePhase == E_GamePhase.Battle)
        {
            // 戦闘処理をする(後々戦闘処理を入れるために書いておく)
            //for()

            int count = 1;
            // どっちかが倒れたら戦闘終了
            foreach (var player in m_playerObjects)
            {
                int playerHp = player.GetComponent<PlayerAvatar>().HP; // プレイヤーのHPをチェックする
                int playerId = player.GetComponent<PlayerAvatar>().Id;
                Debug.Log($"{count}PHP : {playerHp}");
                Debug.Log($"{count}PID : {playerId}");
                if (playerHp <= 0) // プレイヤーが倒れたら
                {
                    m_gamePhase = E_GamePhase.BattleEnd; // ゲームフェーズを結果にする
                    continue;
                }
                count++;
            }

            return;
        }

        // 戦闘終了
        if(m_gamePhase == E_GamePhase.BattleEnd)
        {
            // 開始演出をする(後々演出を入れるために書いておく)
            m_currentBattleEndTextTime += delta;

            if (m_currentBattleEndTextTime < m_maxBattleEndTextTime) return;
            
            // リザルトに向けての準備

            // ○○の勝利って形にしたい
            // どっちかが倒れたら戦闘終了
            int count = 1;
            foreach (var player in m_battlePlayers)
            {
                int playerHp = player.GetComponent<PlayerAvatar>().HP; // プレイヤーのHPをチェックする

                if (playerHp > 0)
                {
                    continue;
                }
                count++;
            }

            m_gamePhase = E_GamePhase.Result;
            return;
        }

        // リザルト
        if(m_gamePhase == E_GamePhase.Result)
        {
            m_currentResultTextTime += delta;

            if (m_currentResultTextTime < m_maxResultTextTime) return;

            m_gamePhase = E_GamePhase.End; // ゲームフェーズを終了にする
            return;
        }

        // 終了
        if(m_gamePhase == E_GamePhase.End)
        {
            Debug.Log("終了状態");
            // 戦闘者が全員リスタートにチェックしたらリスタートする
            foreach (var player in m_battlePlayers)
            {
                bool playerReStartFlag = player.GetComponent<PlayerAvatar>().RestartFlag;

                // どっちかが倒れたら戦闘終了
                int playerId = player.GetComponent<PlayerAvatar>().Id;
                Debug.Log($"{playerId}PHP : {playerReStartFlag}");
                Debug.Log($"{playerId}PID : {playerReStartFlag}");

                // リスタートしない人が一人でもいるならリスタートはまだしない
                if (!playerReStartFlag) return;
            }

            // 全員リスタートすると決めた時(再度Startから開始)
            ReStart();
        }

    }

    // リスタートする関数(RPC化させてみる？)
    private void ReStart()
    {
        Debug.Log("リスタート");
        // 初期化した後、フェーズをStartにする
        m_currentStartTextTime = 0;
        m_currentBattleEndTextTime = 0;
        m_currentResultTextTime = 0;

        // Playerのステータスも初期状態に戻す
        foreach (var player in m_battlePlayers)
        {
            PlayerAvatar playerAvatar = player.GetComponent<PlayerAvatar>();

            playerAvatar.HP = 3;
            player.GetComponent<NetworkObject>().transform.position = playerAvatar.StartPos;
        }

        m_gamePhase = E_GamePhase.Start;
    }

    // リスタートする関数(RPC化させてみる？)
    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    private void ReStartRpc()
    {
        Debug.Log("リスタート");
        // 初期化した後、フェーズをStartにする
        m_currentStartTextTime = 0;
        m_currentBattleEndTextTime = 0;
        m_currentResultTextTime = 0;

        // Playerのステータスも初期状態に戻す
        foreach (var player in m_battlePlayers)
        {
            PlayerAvatar playerAvatar = player.GetComponent<PlayerAvatar>();

            playerAvatar.RestartFlag = false;
            playerAvatar.HP = 3;
            player.GetComponent<NetworkObject>().transform.position = playerAvatar.StartPos;
        }

        m_gamePhase = E_GamePhase.Start;
    }

    // プレイヤーを配列に格納する
    public void SetPlayer(NetworkObject player)
    {
        //int num = m_playerObjects.Count;
        //Debug.Log($"プレイヤーを配列に格納します。現在の配列の長さは{num}です。");
        m_playerObjects.Add(player);
    }

    // バトルするプレイヤーのゲッタ
    public List<NetworkObject> GetBattlePlayers()
    {
        return m_battlePlayers;
    }

    // ゲームフェーズのゲッタ
    public E_GamePhase GetGamePhase()
    {
        return m_gamePhase;
    }
}
