using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeaderBoardController : MonoBehaviour {
    public GameObject leaderBoardBox,buttonCampain, buttonFree;
    public Sprite[] spriteAvatar;
    public void ButtonCampainClick()
    {
        buttonCampain.SetActive(true);
        buttonFree.SetActive(false);
    }
    public void ButtonFreeClick()
    {
        buttonCampain.SetActive(false);
        buttonFree.SetActive(true);
    }
    public void ButtonHideLBClick()
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
