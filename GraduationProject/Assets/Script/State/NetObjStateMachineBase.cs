using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
/// <summary>
/// ネットワーク上で管理するオブジェクト基底クラス(ステートマシン)
/// </summary>

public class NetObjStateMachineBase
{
    // 親オブジェクト
    protected NetworkObject m_parentObject;

    // ステート管理配列
    protected Dictionary<string, NetStateBase> m_stateTypes = new Dictionary<string, NetStateBase>();

    protected NetStateBase m_currentState; // 現在のステート
    protected NetStateBase m_beforState; // 前のステート

    // コンストラクタ
    public NetObjStateMachineBase(NetworkObject parentObject)
    {
        m_parentObject = parentObject;
    }

    // ステートの追加処理
    protected virtual void AddState(string stateName, NetStateBase state)
    {
        // 入れたいステートが同じ名前や型なら通さない
        foreach (var it in m_stateTypes)
        {
            if (it.Key == stateName)
            {
                Debug.LogError("同じ名前のステートが存在します。\n" +
                    $"if(it.Key == stateName：{stateName})" +
                    "void StateMachineBase::AddState(string stateName,StateBase state)");
                return;
            }
            else if (it.Value == state)
            {
                Debug.LogError("同じ型のステートが存在します。\n" +
                    $"if(it.Value == state：{state})\n" +
                    "void StateMachineBase::AddState(string stateName,StateBase state)");
                return;
            }
        }

        m_stateTypes.Add(stateName, state);
    }

    // ステート変更用の関数
    // 引数1 移行したいステートの名前
    public virtual NetStateBase ChangeState(string stateName)
    {
        // フラグとしてステート変更ができているかをチェックする変数を作る
        bool changeStateFlag = false;

        foreach (var it in m_stateTypes)
        {
            if (it.Key == stateName)
            {
                // もし、ステート移行する際にステートが入っていた場合は終了処理をしてからにする
                if (m_currentState != null)
                {
                    // ここに移行前のステートの終了処理を書く
                    m_currentState.OnExit();
                }

                // ステート移行処理
                m_beforState = m_currentState;
                m_currentState = it.Value;
                m_currentState.OnEnter(); // ステートの開始処理をする
                changeStateFlag = true; // ステート変更できている

                return m_currentState;
            }
        }

        if (!changeStateFlag)
        {
            Debug.LogError("指定している名前のステートは存在しません。\n" +
            $"if(it.Key == stateName：{stateName})\n" +
            "StateBase StateMachineBase::ChangeState(string stateName)");
        }

        return null;
    }

    // 現在ステートのゲッタ
    public NetStateBase GetCurrentState()
    {
        return m_currentState;
    }

    // 現在ステート文字列のゲッタ
    public string GetCurrentStateString()
    {
        foreach (var it in m_stateTypes)
        {
            if (it.Value == m_currentState)
            {
                return it.Key;
            }
        }

        return null;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void OnStart()
    {

    }

    // Update is called once per frame
    public virtual void OnUpdate()
    {
        // 更新
        m_currentState.OnUpdate();
    }
}
