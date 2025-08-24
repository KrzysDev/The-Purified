using ThePurified.AudioSystem;
using UnityEngine;


namespace ThePurified.AI 
{
    /// <summary>
    /// klasa uzywana w evencie animacji do puszczania dzwieku kroku clowna.
    /// </summary>
    public class WalkSound : MonoBehaviour
    {
        public void PlayFootstepSound()
        {
            AudioManager.instance.PlayRandomWithTag("clown", transform.position, 0.9f, 1f);
        }
    }
}


