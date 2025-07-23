using Unity.Cinemachine;
using UnityEngine;
public class CinemachineUpdate : MonoBehaviour
{
    private CinemachineBrain brain;
    void Start()
    {
        brain = GetComponent<CinemachineBrain>(); 
    }
    void Update()
    {
        if (brain == null)
            Debug.LogError("there is no brain!");

        brain.ManualUpdate();
    }
}
