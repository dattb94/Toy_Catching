using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneManager : MonoBehaviour {
    public void ButtonCampain()
    {
        Modules.PlayAudio("buttonClick", 0.3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void ButtonFree()
    {
        Modules.PlayAudio("buttonClick", 0.3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    void Update()
    {
        //Set data for setting audio box
        imageThanhKeo.GetComponent<RectTransform>().position = new Vector3(min.position.x + ((max.position.x - min.position.x) * scb.value),
            min.position.y, min.position.z);
        //
    }

    // xu ly setting audio 
    public GameObject settingAudiBox, imageThanhKeo;
    public Scrollbar scb;
    public RectTransform min, max;
    public void ButtonAudioSettingClick()
    {
        Modules.PlayAudio("buttonClick", 0.3f);
        Modules.LoadAudio();
        scb.value = Modules.volume;
        settingAudiBox.SetActive(true);

    }
    public void ButtonSaveClick()
    {
        Modules.PlayAudio("buttonClick", 0.3f);
        Modules.volume = scb.value;
        Modules.SaveVolum();
        settingAudiBox.SetActive(false);
    }
    //
}
