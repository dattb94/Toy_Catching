using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    public void ButtonCampain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void ButtonFree()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
