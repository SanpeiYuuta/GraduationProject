using Fusion;
using NUnit.Framework;
using System.Collections.Generic;
using System.Numerics;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Spawned()
    {
        m_gamePhase = E_GamePhase.Preparation; // ゲームフェーズを準備にする
        Debug.Log("ゲームフェーズは準備です。");
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            Debug.Log("自分が StateAuthority を持っています");
        }
        else
        {
            Debug.Log("自分は StateAuthority を持っていません");
        }

        //Debug.Log($"ゲームフェーズは{m_gamePhase}です。");

        // 戦闘人数に満たしていない時は処理しない
        if (m_playerObjects.Count < m_maxPlayerNum) return;

        //float delta = Runner.DeltaTime;


        // 準備
        if (m_gamePhase == E_GamePhase.Preparation)
        {
            foreach (var player in m_playerObjects)
            {
                // 戦闘するプレイヤーを戦闘用の配列に入れる
                int num = m_battlePlayers.Count;

                // 戦闘人数が最大人数を超えていたら処理しない
                if (num >= m_maxPlayerNum) continue;
                m_battlePlayers.Add(player);
                return;
            }

            Debug.Log("ゲームが開始されます");
            m_gamePhase = E_GamePhase.Start; // ゲームフェーズを開始にする
            return;
        }

        // 開始
        if (m_gamePhase == E_GamePhase.Start)
        {
            // 開始演出をする(後々演出を入れるために書いておく)



            m_gamePhase = E_GamePhase.Battle; // ゲームフェーズを戦闘にする
            return;
        }


        // 戦闘
        if (m_gamePhase == E_GamePhase.Battle)
        {
            // 戦闘処理をする(後々戦闘処理を入れるために書いておく)

            // どっちかが倒れたら戦闘終了
            foreach (var player in m_battlePlayers)
            {
                int playerHp = player.GetComponent<PlayerAvatar>().HP; // プレイヤーのHPをチェックする

                if (playerHp <= 0) // プレイヤーが倒れたら
                {
                    m_gamePhase = E_GamePhase.Result; // ゲームフェーズを結果にする
                    return;
                }
            }

            return;
        }

        // リザルト
        if(m_gamePhase == E_GamePhase.Result)
        {
            m_gamePhase = E_GamePhase.End; // ゲームフェーズを終了にする
            return;
        }

        // 終了
        if(m_gamePhase == E_GamePhase.End)
        {
            // バトルシーンからの遷移

        }

    }

    // プレイヤーを配列に格納する
    public void SetPlayer(NetworkObject player)
    {
        //int num = m_playerObjects.Count;
        //Debug.Log($"プレイヤーを配列に格納します。現在の配列の長さは{num}です。");
        m_playerObjects.Add(player);
    }

    // ゲームフェーズのゲッタ
    public E_GamePhase GetGamePhase()
    {
        return m_gamePhase;
    }
}
