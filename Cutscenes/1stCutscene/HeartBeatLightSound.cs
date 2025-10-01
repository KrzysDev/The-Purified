using ThePurified.AudioSystem;
using UnityEngine;



namespace ThePurified.FX
{
    /// <summary>
    /// posiada publiczna funkcje do odtworzenia dzwieku bicia serca. (cutscenka pierwsza)
    /// </summary>
    public class HeartBeatLightSound : MonoBehaviour
    {        
        public void PlayHeartBeat()
        {
            AudioManager.instance.PlaySound("heartbeat");
        }
    }
}

