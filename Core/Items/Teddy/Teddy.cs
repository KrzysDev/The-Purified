
using ThePurified.PlayerSystem;
using ThePurified.LevelManagement;
using UnityEngine;

namespace ThePurified.Items
{
    public class Teddy : GameItem
    {
        public override void OnItemInteract()
        {
            if(LevelManager.instance.currentLevel == LevelManager.Level.Level1)
            {
                if(LevelManager.instance.GetCurrentQuest() == (int)LevelManager.Level1Quests.FindKey)
                {
                    InspectController.instance.SetItemToInspect(gameObject);
                }
                
            }
            
        }

    }
}

