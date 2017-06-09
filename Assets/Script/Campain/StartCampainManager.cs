using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartCampainManager : MonoBehaviour {
    void Start()
    {
        StartShow();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ResetCampain();
    }
    public void ResetCampain()
    {
        Modules.ResetCampainData();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void StartShow()
    {
        Modules.LoadDataCampain();
        SetDataStack();
    }
    public Transform parentStacks;
    void SetDataStack()
    {
        Modules.LoadDataCampain();
        if (!PlayerPrefs.HasKey("indexCampainNow"))
        {
            Modules.indexCampainNow = 0;
            Modules.SaveIndexCampainNow();
            print("nll");
        }
        for (int i = 0; i < parentStacks.childCount; i++)
        {
            if (i == Modules.indexCampainNow)
            {
                parentStacks.GetChild(i).FindChild("checkMark").gameObject.SetActive(false);
                parentStacks.GetChild(i).FindChild("arrow").gameObject.SetActive(true);
                parentStacks.GetChild(i).FindChild("Button").gameObject.SetActive(true);
                parentStacks.GetChild(i).GetComponent<Image>().color = Color.white;
            }
            else if (i < Modules.indexCampainNow)
            {
                parentStacks.GetChild(i).FindChild("checkMark").gameObject.SetActive(true);
                parentStacks.GetChild(i).FindChild("arrow").gameObject.SetActive(false);
                parentStacks.GetChild(i).FindChild("Button").gameObject.SetActive(false);
                parentStacks.GetChild(i).GetComponent<Image>().color = Color.grey;
            }
            else if (i > Modules.indexCampainNow)
            {
                parentStacks.GetChild(i).FindChild("checkMark").gameObject.SetActive(false);
                parentStacks.GetChild(i).FindChild("arrow").gameObject.SetActive(false);
                parentStacks.GetChild(i).FindChild("Button").gameObject.SetActive(false);
                parentStacks.GetChild(i).GetComponent<Image>().color = Color.green;
            }
        }
    }
    public GameObject StartCampainContain, CamPainContain;
    public void ButtonStackClick()
    {
        StartCampainContain.SetActive(false);
        CamPainContain.SetActive(true);
    }
}
