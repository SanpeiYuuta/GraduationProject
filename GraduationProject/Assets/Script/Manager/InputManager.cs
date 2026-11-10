using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// プレイヤーの入力を管理するマネージャー
/// </summary>
public class InputManager : MonoBehaviour
{
    private NetworkObject m_player;
    
    // ゲーム用のマネージャー
    private GameObject m_gameManager;

    // インプットアクション
    private InputAction m_moveAction;
    private InputAction m_dodgeAction;
    private InputAction m_dashAction;
    private InputAction m_attackAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_moveAction = InputSystem.actions.FindAction("Move");
        //m_inputActions.Add("Move", m_moveAction);
        m_dodgeAction = InputSystem.actions.FindAction("Dodge");
        //m_inputActions.Add("Dodge", m_dodgeAction);
        m_dashAction = InputSystem.actions.FindAction("Dash");
        //m_inputActions.Add("Dash", m_dashAction);
        m_attackAction = InputSystem.actions.FindAction("Attack");
        //m_inputActions.Add("Attack", m_attackAction);

    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーが設定されていない場合は、処理を行わない
        if (!m_player) return;
    }

    // プレイヤーを設定するセッタ
    public void SetPlayer(NetworkObject player)
    {
        // 既に設定されている場合は、再設定しない
        if (m_player) return;

        m_player = player;
    }

}
