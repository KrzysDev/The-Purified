using ThePurified.FX;
using UnityEngine;

namespace ThePurified.AI
{
    /// <summary>
    /// przechowuje funkcje wywolywana za pomoca eventu animacji do trzesienia kamery gracza.
    /// </summary>
    public class WalkShake : MonoBehaviour
    {
        [SerializeField] Transform playerBody;
        [SerializeField] Transform clownAgent;
        [SerializeField] float maxAmplitude;
        [SerializeField] float minAmplitude;
        [SerializeField] float shakeDuration;
        [SerializeField] float shakeFrequency;

        [SerializeField] float maxDistance = 110f;

        /// <summary>
        /// Funkcja trzesaca kamera tym mocniej im blizej gracz znajduje sie chodzacego robota.
        /// </summary>
        public void Shake()
        {
            float distance = Vector3.Distance(playerBody.position, clownAgent.position);

            float t = Mathf.Clamp01(distance / maxDistance);
            float currentShakeAmplitude = Mathf.Lerp(maxAmplitude, minAmplitude, t);

            CameraShake.instance.Shake(shakeDuration, currentShakeAmplitude, shakeFrequency);

        }
    }
}

