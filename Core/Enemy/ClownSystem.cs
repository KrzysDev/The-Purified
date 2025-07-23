using System.Collections;
using ThePurified.Items;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using ThePurified.Ai;

namespace ThePurified.AI
{   
    /// <summary>
    /// klasa obslugujaca logike AI Robo-Clown'a.
    /// </summary>
    [RequireComponent(typeof(ClownRiddle))]
    public class ClownSystem : MonoBehaviour
    {
        [Header("NavMesh Agent")]
        [SerializeField] NavMeshAgent clownAgent;
        [Header("Player: ")]
        [Tooltip("PlayerBody za ktorym AI podaza")]
        [SerializeField] Transform target;

        public static bool activated = true;

        public static ClownSystem instance;

        Coroutine currentCoroutine;


        [Header("Czas deaktywacji")]
        [Tooltip("najkrotszy czas przez jaki clown moze byc wylaczony")]
        [SerializeField] float minTime = 20f;
        [Tooltip("najdluzszy czas przez jaki clown moze byc wylaczony")]
        [SerializeField] float maxTime = 40f;


        [Tooltip("wszystkie generatory na mapie")]
        [SerializeField] List<Generator> generators = new List<Generator>();

        private ClownRiddle ridlde; //zagadka na clownie

        void Start()
        {
            instance = this;

            ridlde = GetComponent<ClownRiddle>();
        }

        public void ActivateAI()
        {
            currentCoroutine = StartCoroutine(ActivateClown());
        }

        private void Update()
        {
            if (!Generator.allGeneratorsAreOff)
            {
                //Debug.Log("sprawdzam czy wszystkie generatory sa wylaczone");
                Generator.allGeneratorsAreOff = AllGeneratorsAreOff();
                //Debug.Log("sprawdzilem");
            }

            if (Generator.allGeneratorsAreOff && activated)
            {
                //Debug.Log("dezaktywuje clown'a");
                StartCoroutine(Deactivate(Random.Range(minTime, maxTime)));

            }
        }

        /// <summary>
        /// Sprawdza czy wszystkie generatory sa wylaczone
        /// </summary>
        /// <returns> prawda jesli wszystkie generatory sa wylaczone </returns>
        bool AllGeneratorsAreOff()
        {
            for (int i = 0; i < generators.Count; i++)
            {
                if (generators[i].active)
                {
                    //Debug.Log("ktorys generator jest ON (zwracam falsz)");
                    return false;
                }

            }
            //Debug.Log("wszystkie sa OFF");
            return true;
        }

        /// <summary>
        /// ustawia cel podazania AI
        /// </summary>
        /// <param name="position"> pozycja za ktora AI podaza </param>
        private void SetDestination(Transform position)
        {
            clownAgent.SetDestination(position.position);
        }

        /// <summary>
        /// korutyna aktywujaca clowna. 
        /// </summary>
        private IEnumerator ActivateClown()
        {
            yield return new WaitForSeconds(2f); //TODO: tutaj jakas animacja wstawania budzenia sie czy cos takiego.

            if (ridlde.inInteraction)
            {
                StartCoroutine(ridlde.ZoomOut());
            }

            while (activated)
            {
                SetDestination(target);
                yield return new WaitForSeconds(0.1f);
            }
        }
        ///<summary>
        ///korutyna usypiajaca clowna
        ///</summary>
        public IEnumerator Deactivate(float seconds)
        {
            //Debug.Log($"Clown deaktywowany na {seconds}");

            activated = false;
            yield return new WaitForSeconds(seconds);

            activated = true;

            //Debug.Log("koniec deaktywacji powrot....");

            ActivateAllGenerators();

            currentCoroutine = StartCoroutine(ActivateClown());
        }

        /// <summary>
        /// aktywuje wszystkie generatory spowrotem
        /// </summary>
        private void ActivateAllGenerators()
        {
            Generator.allGeneratorsAreOff = false;
            for (int i = 0; i < generators.Count; i++)
            {
                generators[i].TurnOn();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StopCoroutine(currentCoroutine);
                //Debug.Log("zlapal gracza!");
            }
        }
    }
}
