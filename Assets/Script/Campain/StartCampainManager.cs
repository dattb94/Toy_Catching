using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartCampainManager : MonoBehaviour
{
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
    public GameObject buttonGift;
    void SetDataStack()
    {
        Modules.LoadDataCampain();
        if (!PlayerPrefs.HasKey("indexCampainNow"))
        {
            Modules.indexCampainNow = 0;
            Modules.SaveIndexCampainNow();
            print("nll");
        }
        if (Modules.indexCampainNow != parentStacks.childCount)
        {
            buttonGift.GetComponent<Image>().color = Color.grey;
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
        else {
            for (int i = 0; i < parentStacks.childCount; i++)
            {
                parentStacks.GetChild(i).FindChild("checkMark").gameObject.SetActive(true);
                parentStacks.GetChild(i).FindChild("arrow").gameObject.SetActive(false);
                parentStacks.GetChild(i).FindChild("Button").gameObject.SetActive(false);
                parentStacks.GetChild(i).GetComponent<Image>().color = Color.grey;
            }
            buttonGift.GetComponent<Image>().color = Color.white;
            buttonGift.GetComponent<Button>().enabled = true;
            buttonGift.GetComponent<Animator>().enabled = true;
        }
    }
    public GameObject StartCampainContain, CamPainContain;
    public void ButtonStackClick()
    {
        Modules.LoadDataCampain();
        infoLebelBox.SetActive(true);
        textLevel.text = "Level: " + Modules.level;
        textScoreTarget.text = "target: " + Modules.scoreNeed + " score";
        StartCoroutine(WaitLoadCampainContain());
    }
    IEnumerator WaitLoadCampainContain()
    {
        yield return new WaitForSeconds(2);
        StartCampainContain.SetActive(false);
        CamPainContain.SetActive(true);
    }
    //xu ly phan bonus gift box
    public GameObject bonusScoreBox;
    public Text textScoreTotalbonusBox;
    public void ButtonGiftClick()
    {
        Modules.LoadDataCampain();
        Modules.scoreTotalCampain += 50000;
        Modules.SaveScoreTotalCampain();
        bonusScoreBox.SetActive(true);
        textScoreTotalbonusBox.text = "Total score: " + Modules.scoreTotalCampain;
        buttonGift.GetComponent<Button>().enabled = false;
        buttonGift.GetComponent<Animator>().enabled = false;
    }
    public void ButtonHideBonusBox()
    {
        Modules.indexCampainNow = 0;
        Modules.SaveIndexCampainNow();
        StartCoroutine(WaitLoadSceneHome());
    }
    IEnumerator WaitLoadSceneHome()
    {
        yield return new WaitForSeconds(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    //
    //Xy ly phan information level box
    public GameObject infoLebelBox;
    public Text textLevel, textScoreTarget;
    //
}
