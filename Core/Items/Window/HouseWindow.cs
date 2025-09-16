using UnityEngine;
using ThePurified.AudioSystem;


namespace ThePurified.Items
{
    /// <summary>
    /// puszcza dzwiek deszczu zza okna
    /// </summary>
    public class HouseWindow : MonoBehaviour
    {
        void Start()
        {
            AudioManager.instance.PlaySoundInPosition("rain", transform.position);
        }
    }
}

