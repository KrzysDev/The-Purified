using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace ThePurified.FX
{
    /// <summary>
    /// Odpowiada za trzesienie kamery.
    /// </summary>
    [RequireComponent(typeof(CinemachineBasicMultiChannelPerlin))]
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake instance;
        CinemachineBasicMultiChannelPerlin perlin;

        private void Start()
        {
            instance = this;
            perlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Shake(0.15f, 5f, 0.5f);
            }
            
        } 

        public void Shake(float duration, float amplitude, float frequency)
        {
            StartCoroutine(ShakeCamera(duration, amplitude, frequency));
        }

        private IEnumerator ShakeCamera(float duration, float amplitude, float frequency)
        {
            float elapsed = 0;

            float currentFrequency = frequency;

            perlin.AmplitudeGain = amplitude;
            perlin.FrequencyGain = frequency;

            while(elapsed < duration)
            {
                elapsed += Time.deltaTime;

                currentFrequency = Mathf.Lerp(frequency, 0, elapsed);

                perlin.FrequencyGain = currentFrequency;

                yield return null;
            }

            perlin.FrequencyGain = 0;
        }
    }
}


