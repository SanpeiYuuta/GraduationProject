using Fusion;
using UnityEngine;

public class PlayerStateAttackBase : PlayerStateBase
{
    // UŒ‚ŠÔ
    protected float m_attackTimeMax = 0.8f; // ‘S‘Ì‚ÌUŒ‚ŠÔ
    protected float m_attackTimeMaxFirst = 0.3f; // ˆêŒ‚–Ú‚ğ•ú‚ÂUŒ‚ŠÔ

    // UŒ‚”­¶ŠÔ
    protected float m_startAttackTime = 0.2f;
    // UŒ‚ŠÔŒv‘ª
    protected float m_countAttackTime = 0.0f;

    // Ÿ‚ÌUŒ‚ƒ{ƒ^ƒ“ó•tŠÔ
    protected float m_graceTimeNextAttack = 0.4f;
    // Ÿ‚ÌUŒ‚‚ÉˆÚs‚·‚é‚©‚Ìƒtƒ‰ƒO
    protected bool m_isNextAttack = false;

    // ƒvƒ‰ƒX‚·‚éUŒ‚—Í
    protected float m_addAttackPower = 0.0f;

    // UŒ‚”»’èoŒ»ƒtƒ‰ƒO
    protected bool m_attackCollisionFlag = false;

    public PlayerStateAttackBase(NetworkObject parentObject, GameObject cameraObject) : base(parentObject, cameraObject)
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
