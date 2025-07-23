using UnityEngine;
using ThePurified.AudioSystem;

namespace ThePurified.Items
{
    public class EntranceDoor : MonoBehaviour
    {
        void Start()
        {
            AudioManager.instance.PlaySoundInPosition("Wind", transform.position);
        }
    }
}


