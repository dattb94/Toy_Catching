using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartCampainManager : MonoBehaviour
{
    void Start()
    {
        SetDataCanvas();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Modules.ResetCampainData();
    }
    public GameObject stackparent, buttonNew, buttonPre, startCampainContain, campainContain;
    public Text textstarNow, textstarTarget, textCampainNow;
    void SetDataCanvas()
    {
        Modules.LoadDataCampain();
        //set data text
        int ii = Modules.campainNow + 1;
        textCampainNow.text = "Campain: " + ii;

        int startNow = 0;
        for (int i = 0; i < Modules.levelCampain; i++)
        {
            startNow += Modules.starStack[i];
        }

        textstarNow.text = "star: " + startNow;
        textstarTarget.text = "target star: " + GetStaTarget();
        //stdata button
        if (Modules.campainNow > 0)
        {
            buttonPre.GetComponent<Image>().color = Color.white;
            buttonPre.GetComponent<Button>().enabled = true;
        }
        print(Modules.campainNow + " " + (int)Modules.levelCampain / 6);
        if (Modules.campainNow < (int)Modules.levelCampain / 6)
        {
            buttonNew.GetComponent<Image>().color = Color.white;
            buttonNew.GetComponent<Button>().enabled = true;
        }
        //set data stackParent
        Transform parent = stackparent.transform;
        for (int i = 0; i < parent.childCount; i++)
        {
            int levelStack = Modules.campainNow * 6 + (i);
            parent.GetChild(i).FindChild("textLevel").GetComponent<Text>().text = (levelStack + 1) + "";
            //print(campainNow * 6 + (i)+"   "+i);
            if (levelStack < Modules.levelCampain)
            {
                parent.GetChild(i).FindChild("imgcenter").GetComponent<Image>().color = Color.green;
                parent.GetChild(i).FindChild("checkmask").gameObject.SetActive(true);
                parent.GetChild(i).FindChild("Button").gameObject.SetActive(true);
                parent.GetChild(i).FindChild("bgstar").gameObject.SetActive(true);

                //print(levelStack + " " + _star[levelStack]);
                for (int j = 0; j < Modules.starStack[levelStack]; j++)
                {
                    parent.GetChild(i).FindChild("bgstar").GetChild(j).gameObject.SetActive(true);
                }
            }
            if (levelStack == Modules.levelCampain)
            {
                parent.GetChild(i).FindChild("imgcenter").GetComponent<Image>().color = Color.white;
                parent.GetChild(i).FindChild("checkmask").gameObject.SetActive(false);
                parent.GetChild(i).FindChild("Button").gameObject.SetActive(true);
            }
            if (levelStack > Modules.levelCampain)
            {
                parent.GetChild(i).FindChild("imgcenter").GetComponent<Image>().color = Color.grey;
                parent.GetChild(i).FindChild("checkmask").gameObject.SetActive(false);
                parent.GetChild(i).FindChild("Button").gameObject.SetActive(false);
            }
        }

    }
    int GetStaTarget()
    {
        return (Modules.campainNow + 1) * 20;
    }
    public void ButtonStackClick(int _index)
    {
        Modules.indexCampainNow = Modules.campainNow * 6 + _index;
        startCampainContain.SetActive(false);
        campainContain.SetActive(true);
    }
    public void ButtonNextClick()
    {
        Modules.LoadDataCampain();
        Modules.campainNow += 1;
        Modules.SaveCampainNow();
        Application.LoadLevel(Application.loadedLevelName);
    }
    public void ButtonPreClick()
    {
        Modules.LoadDataCampain();
        Modules.campainNow -= 1;
        Modules.SaveCampainNow();
        Application.LoadLevel(Application.loadedLevelName);
    }

}
