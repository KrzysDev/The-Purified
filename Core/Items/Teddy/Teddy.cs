
using ThePurified.PlayerSystem;
using ThePurified.LevelManagement;
using UnityEngine;
using ThePurified.AudioSystem;

namespace ThePurified.Items
{
    /// <summary>
    /// klasa obslugujaca logike misia z kluczykiem.
    /// </summary>
    public class Teddy : GameItem
    {
        public override void OnItemInteract()
        {
            if(LevelManager.instance.currentLevel == LevelManager.Level.Level1)
            {
                if(LevelManager.instance.GetCurrentQuest() == (int)LevelManager.Level1Quests.FindKey)
                {
                    AudioManager.instance.PlaySound("pick up");
                    InspectController.instance.SetItemToInspect(gameObject);
                }
                
            }
            
        }

        public override void OnItemInspectionEnd()
        {
            AudioManager.instance.PlaySound("pick up"); 
        }

    }
}

