using UnityEngine;
using ThePurified.AudioSystem;

namespace ThePurified.Items
{
    /// <summary>
    /// Drzwi wejsciowe. Tutaj po prostu puszczamy dzwiek przeciagu.
    /// </summary>
    public class EntranceDoor : MonoBehaviour
    {
        void Start()
        {
            AudioManager.instance.PlaySoundInPosition("Wind", transform.position);
        }
    }
}


