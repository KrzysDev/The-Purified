
using ThePurified.LevelManagement;
using UnityEngine;
using ThePurified.AudioSystem;

namespace ThePurified.Items 
{
    public class Plank : GameItem
    {
        public static int planksTaken = 0;

        private bool interacted = false;

        public override void OnItemInteract()
        {
            if (!interacted)
            {
                interacted = true;
                planksTaken++;

                if(planksTaken != 3)
                {
                    AudioManager.instance.PlaySoundInPosition("plank fall", transform.position, 1f, 1.1f);
                    Destroy(gameObject);
                }
                else
                {
                    LevelManager.instance.NextQuest();
                    BasementDoor.instance.Open();
                    AudioManager.instance.PlaySoundInPosition("plank fall", transform.position, 1f, 1.1f);
                    Destroy(gameObject);
                }
            }
        }

    }
    

}

