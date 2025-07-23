using UnityEngine;
using UnityEngine.Video;

namespace ThePurified.Items
{
    /// <summary>
    /// klasa uzywana przez projektor w jednym z pokoji w grze. Odtwarza film instruktażowy.
    /// </summary>
    public class Projector : MonoBehaviour
    {
        [SerializeField] GameObject projectorLight;

        [Tooltip("Projector board renderer")]
        [SerializeField] Renderer boardRenderer;
        [SerializeField] Material onMaterial;

        [SerializeField] VideoPlayer player;

        bool interacted = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!interacted)
            {
                projectorLight.SetActive(true);
                boardRenderer.material = onMaterial;
                player.Play();

            }


        }

    }   
}
