using System.Collections;
using ThePurified.Items;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using ThePurified.Ai;

namespace ThePurified.AI
{
    [RequireComponent(typeof(ClownRiddle))]
    public class ClownSystem : MonoBehaviour
    {
        [Header("NavMesh Agent")]
        [SerializeField] NavMeshAgent clownAgent;
        [Header("Player: ")]
        [SerializeField] Transform target;

        public static bool activated = true;

        public static ClownSystem instance;

        Coroutine currentCoroutine;


        [Header("Deactivation time")]
        [SerializeField] float minTime = 20f;
        [SerializeField] float maxTime = 40f;


        [SerializeField] List<Generator> generators = new List<Generator>();

        private ClownRiddle ridlde;

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

        private void SetDestination(Transform position)
        {
            clownAgent.SetDestination(position.position);
        }

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
                Debug.Log("zlapal gracza!");
            }
        }
    }
}
