using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoBehaviour,INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner m_networkRunnerPrefab;
    [SerializeField] private NetworkPrefabRef m_playerAvatarPrefab; //プレイヤープレハブ
    [SerializeField] private GameObject m_cameraPrefab; //カメラプレハブ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        // NetworkRunnerを生成する
        var networkRunner = Instantiate(m_networkRunnerPrefab);

        // GameLauncherをNetworkRunnerのコールバックに登録する
        networkRunner.AddCallbacks(this);

        // 共有モードのセッションに参加する
        var startGameArgs = new StartGameArgs
        {
            GameMode = GameMode.Shared,
            PlayerCount = 2,
            //SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        };
        var result = await networkRunner.StartGame(startGameArgs);

        // 結果をデバックログに出す
        Debug.Log(result);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    // プレイヤーがセッションに参加したときに呼ばれるコールバック

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // セッションへ参加したプレイヤーが自分自身かどうかを判定する
        if(player == runner.LocalPlayer)
        {
            // アバターの初期位置を計算する(半径5の円の内部のランダムな点)
            var rand = UnityEngine.Random.insideUnitCircle * 5f;
            var spawnPosition = new Vector3(rand.x, 1.5f, rand.y);

            // 自分自身のアバターをスポーンする
            //runner.Spawn(m_playerAvatarPrefab, spawnPosition, Quaternion.identity);
            runner.Spawn(m_playerAvatarPrefab, spawnPosition, Quaternion.identity, onBeforeSpawned:(_,NetworkObject) =>
            {
                // プレイヤー名のネットワークプロパティの初期値として、ランダムな名前を設定する
                NetworkObject.GetComponent<PlayerAvatar>().NickName = $"Player{UnityEngine.Random.Range(0, 10000)}";
            });
        }
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
    {
        
    }

    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
    {
        
    }
}
