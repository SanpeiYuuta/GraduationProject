using UnityEngine;

public class PlayerAvatarAnimationEventReceiver : MonoBehaviour
{
    [SerializeField] private AudioClip[] m_footstepAudioClipArray;
    [SerializeField] private AudioClip m_landingAudioClip;


    // ‘«‰¹‚ðÄ¶‚³‚¹‚é
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if(animationEvent.animatorClipInfo.weight > 0.5f)
        {
            var index = Random.Range(0, m_footstepAudioClipArray.Length);
            AudioSource.PlayClipAtPoint(m_footstepAudioClipArray[index], transform.position);
        }
    }

    // ’…’n‰¹‚ÌÄ¶
    private void OnLand(AnimationEvent animationEvent)
    {
        if(animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(m_landingAudioClip, transform.position);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
