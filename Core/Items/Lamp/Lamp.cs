using UnityEngine;
using ThePurified.AudioSystem;

namespace ThePurified.Items
{
    public class Lamp : MonoBehaviour
    {
        private AudioSource audioSource;
        void Start()
        {
            audioSource = AudioManager.instance.GetAndPlaySoundInPosition("Lamp", transform.position);
        }
    }
}

