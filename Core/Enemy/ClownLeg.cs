using System.Collections;
using UnityEngine;

namespace ThePurified.AI
{
    public class ClownLeg : MonoBehaviour
    {
        [SerializeField] Transform stepTarget;
        public bool isStepping = false;
        [SerializeField] AnimationCurve steppingCurve;
        [SerializeField] float steppingTime;

        public void Step()
        {
            StartCoroutine(StepCoroutine());
        }

        private IEnumerator StepCoroutine()
        {
            isStepping = true;
            float elapsed = 0;
            Vector3 start = transform.position; 
            while(elapsed < steppingTime)
            {
                float t = steppingCurve.Evaluate(elapsed);
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(start, stepTarget.position, t);
                yield return null;
            }

            transform.position = stepTarget.position;
            isStepping = false;
            
        }


        public float DistanceToTarget()
        {
            return Vector3.Distance(transform.position, stepTarget.position);
        }
    }
}


