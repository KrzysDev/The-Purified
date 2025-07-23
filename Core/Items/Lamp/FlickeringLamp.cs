using System.Collections;
using UnityEngine;

using ThePurified.AudioSystem;

namespace ThePurified.Items
{
    /// <summary>
    /// Migotajaca lampa
    /// </summary>
    public class FlickeringLamp : MonoBehaviour
    {
        [Header("swiatlo lampy")]
        [Tooltip("spot light / point light na scenie pod lampa")]
        [SerializeField] GameObject lampLight;

        [Header("materialy")]
        [Tooltip("material lampy gdy jest wlaczona")]
        [SerializeField] Material onMaterial;
        [Tooltip("material lampy gdy jest wylaczona")]
        [SerializeField] Material offMaterial;

        [Header("Czas")]
        [Tooltip("najdluzszy czas jaki lampa moze sie swiecic")]
        [SerializeField] float maxTime;
        [Tooltip("odwrotnie niz wyzej")]
        [SerializeField] float minTime;


        void Start()
        {
            StartCoroutine(HandleFlicker());
            AudioManager.instance.PlaySoundInPosition("Lamp", transform.position);
        }

        /// <summary>
        /// obsluguje migotanie lampy
        /// </summary>
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


