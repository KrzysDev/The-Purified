using ThePurified.AudioSystem;
using ThePurified.LevelManagement;
using UnityEngine;


namespace ThePurified.FX
{
    /// <summary>
    /// Lekki jumpscare polegajacy na zgaszeniu swiatel gdy gracz wejdzie w trigger.
    /// Dziala tylko gdy gracz jest na konkretnym etapie zadania (szukanie ³omu).
    /// </summary>
    public class HouseLightsOffJumpscare : MonoBehaviour
    {
        [SerializeField] Light[] lightsToDisable;

        [SerializeField] Transform lightBreakPos;

        [SerializeField] Material lampEmissionMaterial;

        bool hadEntered = false; 
            
        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player") && LevelManager.instance.GetCurrentQuest()  == (int)LevelManager.Level1Quests.FindCrowbar && !hadEntered)
            {
                AudioManager.instance.PlaySoundInPosition("LightsOff", lightBreakPos.position);
                DisableLights();

                lampEmissionMaterial.DisableKeyword("_EMISSION");

                AudioSource backgroundAmbience = AudioManager.instance.GetPlayingSoundWithName("ambience");
                AudioSource whiteNoise = AudioManager.instance.GetPlayingSoundWithName("white noise tv");

                Destroy(backgroundAmbience);
                Destroy(whiteNoise);

                hadEntered = true;
            }
        }

        private void DisableLights()
        {
            for(int i = 0; i < lightsToDisable.Length; i++)
            {
                lightsToDisable[i].enabled = false;
            }
        }
    }
}

