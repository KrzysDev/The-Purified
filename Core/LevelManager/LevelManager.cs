using UnityEngine;

namespace ThePurified.LevelManagement
{
    public class LevelManager : MonoBehaviour
    {

        public static LevelManager instance;

        public enum Level
        {
            Level1
        }

        [Tooltip("Ktory poziom jest na tej scenie? Ktora scena w kolejnosci gry to ta scena?")]
        public Level currentLevel;

        public enum Level1Quests
        {
            FindDoor,
            FindKey,
            OpenPadlock,
            FindCrowbar,
            TakeOffPlanksFromDoor,
            OpenDoor
        }

        private int currentQuest;

        private void Start()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }


            if (currentLevel == Level.Level1)
            {
                currentQuest = (int)Level1Quests.FindDoor;
            }
        }

        public void NextQuest()
        {
            
            if(currentLevel == Level.Level1)
            {
                //wszystkie wartosci enuma
                var values = (Level1Quests[])System.Enum.GetValues(typeof(Level1Quests));

                int lastIndex = values.Length - 1;

                if(currentQuest < lastIndex)
                    currentQuest++;
            }
        }

        public int GetCurrentQuest()
        {
            return currentQuest;
        }

        
    }
}

