using UnityEngine;
using ThePurified.AudioSystem;
using ThePurified.LevelManagement;

namespace ThePurified.Items
{
    /// <summary>
    /// kluczyk ktory znajduje sie w skrzynce misia pluszowego na poczatku gry
    /// </summary>
    public class TeddyKey : InspectionItem
    {
        public static bool equipped = false;
        public override void OnPressed()
        {
            AudioManager.instance.PlaySoundInPosition("keys pick up", transform.position);
            LevelManager.instance.NextQuest();
            equipped = true;
            Destroy(gameObject);
        }
    }
}
