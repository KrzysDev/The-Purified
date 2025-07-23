using UnityEngine;

namespace ThePurified.Flashlight
{   
    /// <summary>
    /// obraca latarke za kamera z opoznieniem.
    /// </summary>
    public class FlashlightRotation : MonoBehaviour
    {
        [SerializeField] Transform cinemachineCamera;

        [SerializeField] float rotationLerpSpeed = 5f;

        private void OnEnable()
        {
            transform.rotation = cinemachineCamera.rotation;
        }

        private void HandleItemRotation()
        {
            transform.rotation = Quaternion.Lerp(transform.localRotation, cinemachineCamera.rotation, rotationLerpSpeed * Time.deltaTime);
            transform.position = cinemachineCamera.position;
        }

        void Update()
        {
            HandleItemRotation();
        }

    }
}

