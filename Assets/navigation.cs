using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

}
