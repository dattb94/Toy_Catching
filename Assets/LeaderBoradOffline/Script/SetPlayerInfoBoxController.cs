using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetPlayerInfoBoxController : MonoBehaviour {
    public Text textInputName, textReWrite;
    public GameObject scrollViewBox, contentBox, buttonChoiseAvatar;
    void SetDataScrollViewBox()
    {
        textReWrite.text = "";
        for (int i = 0; i < contentBox.transform.childCount; i++)
        {
            contentBox.transform.GetChild(i).GetComponent<Image>().sprite = Modules.GetAvatar(i);
            if (Modules.avatar == contentBox.transform.GetChild(i).GetComponent<Image>().sprite)
            {
                contentBox.transform.GetChild(i).GetComponent<Image>().color = Color.grey;
                contentBox.transform.GetChild(i).FindChild("checkMark").gameObject.SetActive(true);
            }
            else {
                contentBox.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                contentBox.transform.GetChild(i).FindChild("checkMark").gameObject.SetActive(false);
            }
        }
    }
    public void ButtonChoiseAvatarClick()
    {
        scrollViewBox.SetActive(true);
        SetDataScrollViewBox();
    }
    public void ButtonAvatarClick(int _index)
    {
        Modules.avatar = Modules.GetAvatar(_index);
        Modules.indexAvatar = _index;
        scrollViewBox.SetActive(false);
        buttonChoiseAvatar.GetComponent<Image>().sprite = Modules.GetAvatar(_index);
    }
    public void ButtonSaveClick()
    {
        if (textInputName.text != null && Modules.indexAvatar != 100 && textInputName.text != "")
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Campaign")
            {
                Modules.AddNewDataToLeaderCampain(new PlayerInfor(textInputName.text, Modules.indexAvatar, Modules.scoreTotalCampain, Modules.indexCampainNow));
            }
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "demo")
            {
                Modules.AddNewDataToLeaderFree(new PlayerInfor(textInputName.text, Modules.indexAvatar, Modules.scoreScene));
            }
            textReWrite.text = "Save complete!";
            StartCoroutine(WaitLoadMain());
        }
        else
        {
            textReWrite.text = "name or avatar is null!";
        }
    }
    IEnumerator WaitLoadMain()
    {
        yield return new WaitForSeconds(3);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void ButtonCancerClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
