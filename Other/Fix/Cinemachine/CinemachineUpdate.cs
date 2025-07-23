using Unity.Cinemachine;
using UnityEngine;
/// <summary>
/// klasa manualnie aktualizujaca cinemachine. (LateUpdate oraz FixedUpdate powodowaly bledy i lagi kamery)
/// </summary>
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
