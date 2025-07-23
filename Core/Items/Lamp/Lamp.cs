using UnityEngine;
using ThePurified.AudioSystem;

public class Lamp : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlaySoundInPosition("Lamp", transform.position);
    }
}
