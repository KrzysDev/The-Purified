
using ThePurified.LevelManagement;
using UnityEngine;

namespace ThePurified.Items 
{

    [RequireComponent(typeof(Rigidbody))]
    public class Plank : GameItem
    {
        Rigidbody body;

        public static int planksTaken = 0;

        private bool interacted = false;

        private bool changedQuest = false;
        public override void ItemStart()
        {
            body = GetComponent<Rigidbody>();
            body.isKinematic = true;
        }

        public override void OnItemInteract()
        {
            if (!interacted)
            {
                interacted = true;
                planksTaken++;
                body.isKinematic = false;
            }
        }

        public override void ItemUpdate()
        {
            if(planksTaken == 3 && !changedQuest)
            {
                LevelManager.instance.NextQuest();
                changedQuest = true;
                Debug.Log("mozna juz otworzyc drzwi!");
                BasementDoor.instance.Open();
            }
        }
    }

}

