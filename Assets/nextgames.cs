using UnityEngine;
using UnityEngine.SceneManagement;

public class nextgames : MonoBehaviour
{
    public void GoToScene(string training_camp)
    {
        SceneManager.LoadScene(training_camp);
    }

    public void GoToScene2(string simple_ai)
    {
        SceneManager.LoadScene(simple_ai);
    }
}

