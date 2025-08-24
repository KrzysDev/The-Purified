using System.Collections;
using UnityEngine;

namespace ThePurified.AI
{
    public class ClownProceduralWalking : MonoBehaviour
    {
        [SerializeField] ClownLeg left;
        [SerializeField] ClownLeg right;

        [SerializeField] float stepDistance = 0.2f;
        [SerializeField] float rightLegDelay = 0.5f;

        private void Start()
        {
            StartCoroutine(HandleLegStepping());
        }

        private IEnumerator HandleLegStepping()
        {
            while(true)
            {
                if (!left.isStepping && left.DistanceToTarget() > stepDistance && !right.isStepping)
                {
                    left.Step();
                }
                else if(!right.isStepping && right.DistanceToTarget() > stepDistance && !left.isStepping)
                {
                    right.Step();
                }
                yield return null;
            }
        }




    }
}

