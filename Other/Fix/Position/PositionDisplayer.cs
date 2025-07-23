using System.ComponentModel;
using UnityEngine;

[ExecuteInEditMode]
public class PositionDisplayer : MonoBehaviour
{
    [Header("Global")]
    [SerializeField] private Vector3 global;
    [Header("Local")]
    [SerializeField] private Vector3 local;


    void Update()
    {
        transform.position = global;
        transform.localPosition = local;   
    }

}
