using UnityEngine;

using UnityEngine.SceneManagement;

namespace ThePurified.Ending
{
    public class GameEndingController : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}

