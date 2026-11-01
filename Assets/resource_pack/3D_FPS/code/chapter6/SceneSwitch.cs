using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class SceneSwitch : MonoBehaviour
{
    public TMP_Dropdown sceneDropdown; 

    public void SwitchScene(int index)
    {
   
        string sceneName = sceneDropdown.options[index].text;

       
        Time.timeScale = 1f;

      
        SceneManager.LoadScene(sceneName);
    }
}
