using ThePurified.Items;
using UnityEngine;
using ThePurified.AudioSystem;

/// <summary>
/// Kluczyk ktory znajduje sie w klaunowym nosie na samym poczatku gry.
/// </summary>
public class ClownsNoseKey : InspectionItem
{
    [SerializeField] Door door;
    public override void OnPressed()
    {
        AudioManager.instance.PlaySoundInPosition("keys pick up", transform.position);
        door.doorUnlocked = true;
        Destroy(gameObject);
    }
}
