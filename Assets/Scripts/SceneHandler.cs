using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    /**
     * used for button navigatoin
     */
    public void toScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
