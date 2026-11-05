using Fusion;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackDeta", menuName = "Scriptable Objects/AttackDeta")]
public class AttackDeta : ScriptableObject
{
    public string attackName; // 攻撃の名前

    public AttackType attackType; // 攻撃の種類(遠距離か近距離か的な)
    //// 誰が攻撃したか(攻撃した瞬間は同期されてなければならないためNetWork限定)
    //public NetworkObject attacker;

    // 喰らった際のスタン時間
    public float stunTime; // スタン時間

    // ヒットストップ時間
    public float hitStopTime; // ヒットストップ時間

    public int attackPower; // 攻撃力
    public float knockbackForce; // ノックバックの強さ
    public Vector3 knockbackDirection; // ノックバックの方向

    // 鳴らすSEの種類(後々追加するかも)

}
