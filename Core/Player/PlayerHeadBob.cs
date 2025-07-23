using UnityEngine;

namespace ThePurified.PlayerSystem
{
    public class PlayerHeadBob : MonoBehaviour
    {

        public static bool headBobEnabled = true;

        private float x, y;
        
        [Header("Player Headbob settings")]
        [Space(3)]
        [SerializeField] float bobSpeed = 1;
        [SerializeField] float bobAmount = 0.01f;
        [SerializeField] float bobRunSpeed = 3f;
        [SerializeField] float bobRunAmount = 0.03f;

        [Header("Player head")]
        [SerializeField] Transform playerHead;

        private Vector3 startingPoint;

        private Vector3 lastPos;

        void Start()
        {
            startingPoint = playerHead.transform.localPosition;
            lastPos = transform.position;
        }

        private void Update()
        {
            if(headBobEnabled)
                HandleHeadBob();
        }

        ///<summary>
        ///Tworzy efekt head bob na obiekcie ktory sledzi cinemachine camera. Zmienia sie dynamiczne w zaleznosci od tego co gracz robi. 
        ///</summary>
        private void HandleHeadBob()
        {
            if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && transform.position != lastPos)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {

                    x += Time.deltaTime * bobRunSpeed;
                    y = Mathf.Sin(x) * bobRunAmount;
                }
                else
                {
                    x += Time.deltaTime * bobSpeed;
                    y = Mathf.Sin(x) * bobAmount;
                }
            }
            else
            {
                y = Mathf.Lerp(y, 0, Time.deltaTime * 5f);
            }

            playerHead.transform.localPosition = new Vector3(startingPoint.x, startingPoint.y + y, startingPoint.z);

            lastPos = transform.position;
        }
    }    
}

