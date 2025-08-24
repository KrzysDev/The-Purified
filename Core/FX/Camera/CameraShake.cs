using System.Collections;
using UnityEngine;

namespace ThePurified.FX
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake instance;

        private void Start()
        {
            instance = this;
        }

        /*private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Shake(0.15f, 0.2f);
            }
            
        }*/

        public void Shake(float duration, float amount)
        {
            StartCoroutine(ShakeCamera(duration, amount));
        }

        private IEnumerator ShakeCamera(float duration, float amount)
        {
            float elapsed = 0;

            Vector3 start = transform.localPosition;

            while(elapsed < duration)
            {
                transform.localPosition = start + new Vector3(Random.Range(-1, 1) * amount, Random.Range(-1, 1) * amount, start.z);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = start;
        }
    }
}


