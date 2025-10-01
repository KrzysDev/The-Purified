using UnityEngine;


namespace ThePurified.AudioSystem
{
    public class PositionAudioPlayer : MonoBehaviour
    {
        [SerializeField] string audioName;

        private void Start()
        {
            AudioManager.instance.PlaySoundInPosition(audioName, transform.position);
        }
    }
    
}

