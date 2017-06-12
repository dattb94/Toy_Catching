using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneManager : MonoBehaviour {
    public void ButtonCampain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void ButtonFree()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    void Update()
    {
        SetDaSettingAudioBox();
    }
    public Scrollbar scb;
    public RectTransform min, max;
    void SetDaSettingAudioBox()
    {
        GetComponent<RectTransform>().position = new Vector3(min.position.x + ((max.position.x - min.position.x) * scb.value),
            min.position.y, min.position.z);
        Modules.volume = scb.value;
        Modules.SaveVolum();
    }
}
