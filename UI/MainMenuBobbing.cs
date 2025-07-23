using UnityEngine;


namespace ThePurified.UI
{
    [RequireComponent(typeof(Camera))]
    public class MainMenuBobbing : MonoBehaviour
    {
        [Header("View Bobbing Settings")]
        [Range(0, 10f)]
            [SerializeField] float bobbingSpeed;
        [Range(0, 1f)]
            [SerializeField] float bobbingAmount;
        [Range(0, 1f)]
            [SerializeField] float sideBobbingAmount; 
        Vector3 newPos;
        Vector3 startingPos;
        private float timer;
        public float transitionSpeed;

        private void Start()
        {
            startingPos = transform.localPosition;
            timer = Mathf.PI / 2;
        }

        private void Update()
        {
            HandleBobbing();
        }

        void HandleBobbing()
        {
            UpdateTimer();
            HandleTimerLimit();
            StartBobbing();
        }

        void UpdateTimer()
        {
            timer += Time.deltaTime * bobbingSpeed;
        }

        void HandleTimerLimit()
        {
            if (timer > Mathf.PI * 2)
                timer = 0f;
        }

        void StartBobbing()
        {
            float sideBobbing = Mathf.Sin(timer) * sideBobbingAmount;

            newPos = new Vector3(startingPos.x + sideBobbing, Mathf.Abs(startingPos.y + Mathf.Sin(timer) * bobbingAmount), startingPos.z);

            transform.localPosition = newPos;
        }
    }
}

