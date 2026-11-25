using UnityEngine;
/// <summary>
/// サンドバックのステートマシン
/// </summary>

public class PunchingBagStateMachineBase : StateMachineBase
{
    public PunchingBagStateMachineBase(GameObject parentObject) : base(parentObject)
    {
        AddState("Default", new PunchingBagDefaultState(parentObject));
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
