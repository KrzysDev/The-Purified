using ThePurified.LevelManagement;

namespace ThePurified.Items
{
    public class Crowbar : GameItem
    {
        bool interacted = false;
        public override void OnItemInteract()
        {
            if(!interacted && LevelManager.instance.currentLevel == LevelManager.Level.Level1 && 
                LevelManager.instance.GetCurrentQuest() == (int)LevelManager.Level1Quests.FindCrowbar)
            {
                LevelManager.instance.NextQuest();
                interacted = true;
                Destroy(gameObject);
            }
            
        }
    }
}

