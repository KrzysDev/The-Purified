using ThePurified.Items;
using UnityEngine;

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
