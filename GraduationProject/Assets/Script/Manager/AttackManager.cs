using Fusion;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 攻撃全般を管理するマネージャー
/// </summary>


public class AttackManager : MonoBehaviour
{
    // プレイヤーの一段目攻撃
    [SerializeField] GameObject m_playerAttackFirst;

    // 攻撃を保管する配列
    private Dictionary<string, GameObject> m_attackArray = new Dictionary<string,GameObject>();

    // プレイヤー
    private NetworkObject m_player;

    private void Awake()
    {
        Debug.Log("Awake通ったよ");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // プレイヤー一段目攻撃登録
        string attackName = m_playerAttackFirst.GetComponent<DamageObject>().HitInfo.attackName;
        Debug.Log(attackName);
        m_attackArray.Add(attackName, m_playerAttackFirst);

    }

    // Update is called once per frame
    void Update()
    {

    }

    // ステージに攻撃オブジェクトを生成する
    // attackName：生成したい攻撃オブジェクトの名前
    // createPos：生成した際の位置 デフォルト値(0,0,0)
    // createQt：生成した際の回転 デフォルト値null 実質Quaternion.identity
    public GameObject CreateAttackObject(string attackName, Vector3 createPos = default, Quaternion? createQt = null)
    {
        // エラー
        if (!m_attackArray.TryGetValue(attackName, out var prefab))
        {
            Debug.LogWarning($"攻撃オブジェクトが見つかりません：{attackName}");
            return null;
        }

        // createQtがnullの場合はQuaternion.identityにする
        var qt = createQt ?? Quaternion.identity;

        return Instantiate(m_attackArray[attackName], createPos, qt);
    }

    // ステージに攻撃オブジェクトを生成する(ネットワーク版)
    // attackName：生成したい攻撃オブジェクトの名前
    // createPos：生成した際の位置 デフォルト値(0,0,0)
    // createQt：生成した際の回転 デフォルト値null 実質Quaternion.identity
    public NetworkObject CreateAttackObjectNetwork(string attackName, Vector3 createPos = default, Quaternion? createQt = null)
    {
        // エラー
        if (!m_attackArray.TryGetValue(attackName, out var prefab))
        {
            Debug.LogWarning($"攻撃オブジェクトが見つかりません：{attackName}");
            return null;
        }

        // createQtがnullの場合はQuaternion.identityにする
        var qt = createQt ?? Quaternion.identity;

        return m_player.Runner.Spawn(m_attackArray[attackName], createPos, qt);
    }

    // 攻撃プレハブの取得
    // 取得したい攻撃プレハブの名前
    public GameObject GetAttackPrefab(string attackName)
    {
        // エラー
        if (!m_attackArray.TryGetValue(attackName, out var prefab))
        {
            Debug.LogWarning($"攻撃プレハブが見つかりません：{attackName}");
            return null;
        }

        return m_attackArray[attackName];
    }

    // 攻撃データの取得
    // 取得したい攻撃データの名前
    public AttackDeta GetAttackDeta(string attackName)
    {
        // エラー
        if (!m_attackArray.TryGetValue(attackName, out var prefab))
        {
            Debug.LogWarning($"攻撃データが見つかりません：{attackName}");
            return null;
        }

        return m_attackArray[attackName].GetComponent<DamageObject>().HitInfo;
    }

    // プレイヤーを設定するセッタ
    public void SetPlayer(NetworkObject player)
    {
        // 既に設定されている場合は、再設定しない
        if (m_player) return;

        m_player = player;
    }
}
