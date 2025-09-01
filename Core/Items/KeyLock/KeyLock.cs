using UnityEngine;
using ThePurified.LevelManagement;

namespace ThePurified.Items
{
    public class KeyLock : GameItem
    {
        bool interacted = false;
        public override void OnItemInteract()
        {
            if (TeddyKey.equipped && !interacted)
            {
                //TODO: jakis dzwiek.
                LevelManager.instance.NextQuest();
                Debug.Log("obecny quest: " + LevelManager.instance.GetCurrentQuest());
                Destroy(gameObject);
                interacted = true;
            }
        }
    }
}

