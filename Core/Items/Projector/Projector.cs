using UnityEngine;
using UnityEngine.Video;

namespace ThePurified.Items
{
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
