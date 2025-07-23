using System.Collections;
using UnityEngine;

using ThePurified.AudioSystem;

namespace ThePurified.Items
{
    public class FlickeringLamp : MonoBehaviour
    {
        [Header("Lamp's light")]
        [SerializeField] GameObject lampLight;

        [Header("Materials")]
        [SerializeField] Material onMaterial;
        [SerializeField] Material offMaterial;

        [Header("Time")]
        [SerializeField] float maxTime;
        [SerializeField] float minTime;


        void Start()
        {
            StartCoroutine(HandleFlicker());
            AudioManager.instance.PlaySoundInPosition("Lamp", transform.position);
        }

        private IEnumerator HandleFlicker()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minTime, maxTime));

                lampLight.SetActive(false);
                gameObject.GetComponent<Renderer>().material = offMaterial;

                yield return new WaitForSeconds(0.1f);

                gameObject.GetComponent<Renderer>().material = onMaterial;
                lampLight.SetActive(true);
            }

        }
    }
}


