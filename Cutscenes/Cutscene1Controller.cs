using System.Collections;
using UnityEngine;

namespace ThePurified.Cutscenes
{
    public class Cutscene1Controller : MonoBehaviour
    {
        [SerializeField] Transform[] cutscenePoints;
        private int i = -1;

        [SerializeField] float zoomingSpeed;

        private void Start()
        {
            StartCoroutine(ZoomIn());
        }

        public void MoveToAnotherPoint()
        {
            i++;

            if(i > cutscenePoints.Length - 1)
            {
                Debug.LogError($"there is no cutscene point of index {i}");
            }
            else
            {
                transform.position = cutscenePoints[i].position;
                transform.rotation = cutscenePoints[i].rotation;
            }
        }

        private IEnumerator ZoomIn()
        {
            while(true)
            {
                transform.position = new Vector3(transform.position.x + Time.deltaTime * zoomingSpeed, transform.position.y, transform.position.z + Time.deltaTime * zoomingSpeed);
                yield return null;
            }
        }
    }
}

