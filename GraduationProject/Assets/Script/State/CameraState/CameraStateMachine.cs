using Fusion;
using UnityEngine;

public class CameraStateMachine : StateMachineBase
{

    // コンストラクタ
    public CameraStateMachine(NetworkObject parentObject):base(parentObject)
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        base.OnUpdate();
    }
}
