using UnityEngine;
/// <summary>
/// カメラのステートの基盤となるクラス
/// </summary>

public class CameraStateBase : MonoBehaviour
{
    Camera m_mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
