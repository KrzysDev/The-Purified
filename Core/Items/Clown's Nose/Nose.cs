using ThePurified.AudioSystem;
using ThePurified.Items;
using ThePurified.PlayerSystem;
using UnityEngine;

/// <summary>
/// klasa uzywana przez nos ktory gracz podnosi na samym poczatku gry zeby go obrocic i wyjac z niego kluczyk.
/// </summary>
public class Nose : GameItem
{
    public override void OnItemInteract()
    {
        if (InspectController.instance.inspectPoint.childCount > 0)
        {
            if (InspectController.instance.inspectPoint.GetChild(0).gameObject != gameObject)
            {
                InspectController.instance.SetItemToInspect(gameObject);
            }
        }
        else
        {
            InspectController.instance.SetItemToInspect(gameObject);
        }

    }


    public override void OnItemInspection()
    {
        AudioManager.instance.PlaySoundInPosition("clowns nose", transform.position, 0.9f, 1.1f);
    }

    public override void OnItemInspectionEnd()
    {
        AudioManager.instance.PlaySoundInPosition("clowns nose", transform.position, 0.9f, 1.1f);
    }

}
