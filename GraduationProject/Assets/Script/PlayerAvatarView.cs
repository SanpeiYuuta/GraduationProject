using TMPro;
using UnityEngine;

public class PlayerAvatarView : MonoBehaviour
{
    GameObject m_camera;

    [SerializeField] private TextMeshPro m_nameLabel; // 名前ラベルのTextMeshProコンポーネント

    // プレイヤーの名前を設定するメソッド
    public void SetNickName(string nickName)
    {
        m_nameLabel.text = nickName;
    }

    private void LateUpdate()
    {
        // プレイヤー名のテキストを、ビルボードにする
        m_nameLabel.transform.rotation = Camera.main.transform.rotation;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // カメラを取得して、カメラの中心点をこのオブジェクトに設定する
        m_camera = GameObject.Find("Camera");
        if (m_camera != null)
        {
            m_camera.GetComponent<CameraMove>().SetOriginObject(gameObject);
        }

        // アバターの初期位置を計算する(半径5の円の内部のランダムな点)
        var rand = UnityEngine.Random.insideUnitCircle * 5f;
        var spawnPosition = new Vector3(rand.x, 2.0f, rand.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
