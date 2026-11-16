using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] Sprite m_katana; // 刀アイコン
    [SerializeField] Sprite m_dagger; // 短剣アイコン
    [SerializeField] Sprite m_greatsword; // 大剣アイコン

    private NetworkObject m_player; // プレイヤー(自機)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var uimanager = GameObject.Find("BattleUIManager").GetComponent<BattleUIManager>();
        m_player = uimanager.PlayerObj;

        // プレイヤーがいなければ更新しない
        if (!m_player) return;

        var avater = m_player.GetComponent<PlayerAvatar>();

        // 装備を取得
        var loadout = avater.GetPlayerLoadout();
        var currentNum = loadout.m_currentWeaponNum;
        var currentWeapon = loadout.m_gearWeaponList[currentNum];

        // 自分のイメージ取得
        var image = GetComponent<Image>();
        var weponText = transform.Find("WeaponTextUI").GetComponent<TextMeshProUGUI>();

        // 持っている武器によってUIを変える
        switch(currentWeapon)
        {
            case E_weapon.Katana:
                //image = m_katana;
                image.sprite = m_katana;
                break;
            case E_weapon.Dagger:
                image.sprite = m_dagger;
                break;
            case E_weapon.Greatsword:
                image.sprite = m_greatsword;
                break;

        }
        // 持っている武器に合わせてテキストも変える
        weponText.text = currentWeapon.ToString();

    }
}
