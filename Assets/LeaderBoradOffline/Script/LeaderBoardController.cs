﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeaderBoardController : MonoBehaviour {
    public GameObject leaderBoardBox,buttonCampain, buttonFree;
    public Sprite[] spriteAvatar;
    void Update()
    {
        print(Modules.lbShow);
        leaderBoardBox.GetComponent<Animator>().SetBool("show",Modules.lbShow);
    }
    public void ButtonCampainClick()
    {
        Modules.PlayAudio("buttonClick", 0.3f);
        buttonCampain.SetActive(true);
        buttonFree.SetActive(false);
    }
    public void ButtonFreeClick()
    {
        Modules.PlayAudio("buttonClick", 0.3f);
        buttonCampain.SetActive(false);
        buttonFree.SetActive(true);
    }
    public void ButtonHideLBClick()
    {
        Modules.PlayAudio("buttonClick", 0.3f);
        Modules.lbShow = false;
        StartCoroutine(WaithideLB());
    }
    IEnumerator WaithideLB()
    {
        yield return new WaitForSeconds(0.4f);
        {
            leaderBoardBox.SetActive(false);
            Modules.pauseGame = false;
            if (FindObjectOfType<CampaignSceneManager>())
                FindObjectOfType<CampaignSceneManager>().WhenPauseGame();
            else if (FindObjectOfType<GameManager>())
            {
                FindObjectOfType<GameManager>().WhenPauseGame();
            }
        }
    }
}
