using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ThePurified.UI
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Vector3 defaultPos;

        [SerializeField] float moveAmount = 10f;

        [Tooltip("How fast button interpolates when hovered over with mouse")]
        [SerializeField] private float animationTime = 0.5f;
        private float elapsed = 0f;

        private Vector3 start;
        

        Coroutine currentCouroutine;

        void Start()
        {
            defaultPos = transform.localPosition;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log(gameObject.name + " - OnPointerEnter");
            if (currentCouroutine != null) StopCoroutine(currentCouroutine);

            currentCouroutine = StartCoroutine(Lerp(new Vector3(defaultPos.x + moveAmount, defaultPos.y, defaultPos.z)));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log(gameObject.name + " - OnPointerExit");
            if (currentCouroutine != null) StopCoroutine(currentCouroutine);

            currentCouroutine = StartCoroutine(Lerp(defaultPos));
        }

        private IEnumerator Lerp(Vector3 pos)
        {
            elapsed = 0f;
            start = transform.localPosition;
            while (elapsed < animationTime)
            {
                transform.localPosition = Vector3.Lerp(start, pos, elapsed/animationTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
