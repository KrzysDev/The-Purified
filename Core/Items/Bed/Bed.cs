using System.Collections;
using UnityEngine;
using ThePurified.PlayerSystem;
using ThePurified.LevelManagement;

namespace ThePurified.Items
{
    /// <summary>
    /// klasa uzywana przez lozko pod ktorym jest lom w pierwszym poziomie gry.
    /// </summary>
    public class Bed : GameItem
    {
        [SerializeField] Transform playerHead;
        [SerializeField] Transform zoomPoint;
        [SerializeField] AnimationCurve curve;

        private Vector3 originalPos;

        bool isZooming = false;

        bool zoomedIn = false;

        public override void OnItemInteract()
        {
            if(LevelManager.instance.currentLevel == LevelManager.Level.Level1 && LevelManager.instance.GetCurrentQuest() == (int)LevelManager.Level1Quests.FindCrowbar)
            {
                if (!isZooming)
                {
                    //Debug.Log("interakcja z lozkiem");
                    originalPos = playerHead.position;
                    StartCoroutine(Zoom(zoomPoint.position));

                    PlayerHeadBob.headBobEnabled = false;
                    PlayerMovement.movementEnabled = false;

                    zoomedIn = true;
                }
            }
            
        }

        public override void ItemUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Tab) && !isZooming && zoomedIn)
            {
                StartCoroutine(Zoom(originalPos));
            }
        }

        private IEnumerator Zoom(Vector3 pos)
        {
            /*if (pos == zoomPoint.position)
                Debug.Log("przyblizam do pozycji zoomPoint");
            else
                Debug.Log("przyblizam do pozycji oryginalnej: " + pos);*/

            isZooming = true;

            float elapsed = 0f;

            Vector3 start = playerHead.position;

            while(elapsed < 1f)
            {
                float t = curve.Evaluate(elapsed / 1f);
                playerHead.position = Vector3.Lerp(start, pos, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            playerHead.position = pos;

            isZooming = false;

            if(pos == originalPos)
            {
                PlayerHeadBob.headBobEnabled = true;
                PlayerMovement.movementEnabled = true;
            }
        }
    }
}

