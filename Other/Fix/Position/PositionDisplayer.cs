using System.ComponentModel;
using UnityEngine;

//Gdy obiekt jest dzieckiem wyswietla sie pozycja lokalna, a ja potrzebowalem pozycjli globalnej bez wyswietlania tej lokalnej. Nie jest to skrypt uzywany w grze juz.

[ExecuteInEditMode]
public class PositionDisplayer : MonoBehaviour
{
    [Header("Global")]
    [SerializeField] private Vector3 global;
    [Header("Local")]
    [SerializeField] private Vector3 local;


    void Update()
    {
        global = transform.position;
        local = transform.localPosition;   
    }

}
