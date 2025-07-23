using UnityEngine;
using UnityEngine.SceneManagement;
public class GameEnder : MonoBehaviour
{
    public void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
