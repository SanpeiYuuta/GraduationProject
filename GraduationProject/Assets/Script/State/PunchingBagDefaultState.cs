using UnityEngine;
/// <summary>
/// サンドバックのデフォルトステート(何もしない)
/// </summary>

public class PunchingBagDefaultState : PunchingBagBaseState
{
    public PunchingBagDefaultState(GameObject parentObject) : base(parentObject)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
