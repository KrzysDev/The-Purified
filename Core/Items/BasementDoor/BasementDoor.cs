using UnityEngine;
using ThePurified.LevelManagement;
using ThePurified.PlayerSystem;
using ThePurified.Flashlight;

namespace ThePurified.Items
{
    public class BasementDoor : GameItem
    {

        [SerializeField] Animator playerAnimator;

        public static BasementDoor instance;

        bool interacted = false;

        public override void ItemStart()
        {
            instance = this;

            playerAnimator.enabled = false;
        }

        public override void OnItemInteract()
        {
            if (LevelManager.instance.currentLevel == LevelManager.Level.Level1 && !interacted)
            {
                Debug.Log("costam costam znalazl drzwi pozniej sie tu cos fajnego zrobi");
                LevelManager.instance.NextQuest();
                interacted = true;
            }
           
        }

        public void Open()
        {
            playerAnimator.enabled = true;
            playerAnimator.SetBool("door unlocked", true);
        }
    }
}

