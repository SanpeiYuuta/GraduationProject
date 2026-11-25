using Fusion;
using UnityEngine;

public class CameraStateMachine : StateMachineBase
{

    // コンストラクタ
    public CameraStateMachine(GameObject parentObject) : base(parentObject)
    {
        AddState("Default", new CameraStateDefault(parentObject));
        AddState("Gun", new CameraStateGun(parentObject));
        ChangeState("Default");
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
