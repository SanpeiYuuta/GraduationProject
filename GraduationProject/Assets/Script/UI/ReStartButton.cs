using Fusion;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// リスタート用のボタン
/// </summary>
public class ReStartButton : MonoBehaviour
{
    private NetworkObject m_parentObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_parentObject) return;

        //// ボタンが押された時リスタートフラグをオンにする
        //if(button.IsPressed())
        //{
        //    Debug.Log("リスタートボタンが押された");
        //    m_parentObject.GetComponent<PlayerAvatar>().RestartFlag = true;
        //}
    }

    // ボタンが押されたときの関数
    public void OnPressedButton()
    {
        Debug.Log("リスタートボタンが押された");
        Debug.Log(m_parentObject);
        m_parentObject.GetComponent<PlayerAvatar>().RestartFlag = true;
    }

    // 親オブジェクトのセッタ
    public void SetParentObject(NetworkObject parentObj)
    {
        // 元々親オブジェクトが居た場合は通さない(上書きはできない)
        if (m_parentObject) return;

        m_parentObject = parentObj;
    }
}
