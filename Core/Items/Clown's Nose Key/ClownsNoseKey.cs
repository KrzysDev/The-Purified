using ThePurified.Items;
using UnityEngine;

/// <summary>
/// Kluczyk ktory znajduje sie w klaunowym nosie na samym poczatku gry.
/// </summary>
public class ClownsNoseKey : InspectionItem
{
    [SerializeField] Door door;
    public override void OnPressed()
    {
        door.doorUnlocked = true;
        //TODO: play some audio.
        Destroy(gameObject);
    }
}
