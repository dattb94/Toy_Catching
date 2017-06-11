using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    public Transform[] listLocal;
    public static float timeSpawn = 0;
    float TimeSpawn = 10;
    public static bool spawn = false;
    public GameObject goChoise;
    public Row[] rows = new Row[9];
    public List<GameObject> listPick;
    float timeGame = 0;
    float loadRows = 0;
    public static bool beLost = false;
    public GameObject lostBox;
    void Start()
    {
        Modules.SetBarrierTop(barrierTop);
        Modules.isCanPick = true;
        Modules.pauseGame = false;
        Modules.LoadDataFree();
        Modules.scoreScene = Modules.scoreNowFree;
        Modules.keepItem = false;
        Modules.LoadLeaderBoardFree();
        beLost = false;
        timeSpawn = TimeSpawn - 1;
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i] = new Row(new List<GameObject>());
        }
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
            Modules.ResetLeaderBoard();
        if (Input.GetKeyDown(KeyCode.A))
            Modules.AddNewDataToLeaderFree(new PlayerInfor("aaa", 2, 300));
        SetDataInfogame();
        if (Modules.pauseGame)
            return;
        SetDataButtonPause();
        CalcuLevel();
        if (beLost)
        {
            lostBox.SetActive(true);
            return;
        }

        if (Modules.keepItem)
        {
            Modules.countBeChoise = 0;
        }
        else Modules.CalcuItemBeChoise(listLocal);
        timeGame += Time.deltaTime;
        if (timeGame >= 0.5f)
        {
            if (timeGame < 0.6f)
            {
                timeGame = Modules.timeNowFree;
            }
        }
        timeSpawn += Time.deltaTime;
        if (timeSpawn > TimeSpawn)
        {
            Modules.Spawn(listLocal);
            timeSpawn = 0;
        }
        else Modules.isSpawning = false;
        Modules.SetChoisePosition(goChoise, listLocal);
        if (Input.GetMouseButtonDown(0))
        {
            if (!Modules.keepItem)
            {
                if (listLocal[Modules.localMouse - 1].childCount > 0)
                {
                    Modules.PickItem(Modules.localMouse - 1, listPick, rows, goChoise, listLocal);
                }
            }
            else
            if (FindObjectOfType<ItemImage>())
            {
                Modules.ThrowItem(Modules.localMouse - 1, rows, listPick, listLocal);
                Modules.isCanPick = false;
                StartCoroutine(Modules.WaitIsCanPick());
            }

        }
        loadRows -= Time.deltaTime;
        if (loadRows <= 0)
        {
            for (int i = 0; i < rows.Length; i++)
            {
                Modules.Row(i, rows, listLocal);
            }
            loadRows = 0.5f;
        }
    }
    void CalcuLevel()
    {
        if (timeGame < 0.5f)
            TimeSpawn = 0.1f;
        if (timeGame < 20 && timeGame >= 0.5f)
        {
            Modules.levelFreeNow = 1;
            TimeSpawn = 10;
        }
        if (timeGame > 20 && timeGame < 40)
        {
            Modules.levelFreeNow = 2;
            TimeSpawn = 9;
        }
        if (timeGame > 40 && timeGame < 60)
        {
            Modules.levelFreeNow = 3;
            TimeSpawn = 8;
        }
        if (timeGame > 60 && timeGame < 80)
        {
            Modules.levelFreeNow = 4;
            TimeSpawn = 7;
        }
        if (timeGame > 80)
        {
            Modules.levelFreeNow = 5;
            TimeSpawn = 6;
        }
    }
    //Set data info game
    public Text txtScoreFree, txtTimePlay, txtBestScore;
    public Image imgTimeSpaw;
    void SetDataInfogame()
    {
        if (Modules.leaderFree.Count > 0)
            txtBestScore.text = "Best: " + Modules.leaderFree[0].score;
        else txtBestScore.text = "Best: " + 0;
        txtScoreFree.text = "Score: " + Modules.scoreScene;
        txtTimePlay.text = "Time: " + (int)timeGame;
        imgTimeSpaw.fillAmount = timeSpawn / TimeSpawn;
    }
    //
    //xu ly nut' pause game
    public GameObject buttonPause, pauseBox;
    public Sprite sprPause, sprPlay;
    public void SetDataButtonPause()
    {
        if (Modules.pauseGame == true)
        {
            buttonPause.GetComponent<Image>().sprite = sprPlay;
        }
        else buttonPause.GetComponent<Image>().sprite = sprPause;
    }
    public void ButtonPauseClick()
    {
        if (Modules.pauseGame == true)
        {
            Modules.pauseGame = false;
            pauseBox.SetActive(false);
            Modules.ThrowItem(Modules.localMouse - 1, rows, listPick, listLocal);
            Modules.keepItem = false;
        }
        else {
            Modules.pauseGame = true;
            pauseBox.SetActive(true);
            Modules.ThrowItem(Modules.localMouse - 1, rows, listPick, listLocal);
            Modules.keepItem = false;
            listPick.Clear();
        }
    }
    public void WhenPauseGame()
    {
        Modules.ThrowItem(Modules.localMouse - 1, rows, listPick, listLocal);
        Modules.keepItem = false;
    }
    //
    //xu ly nut home
    public GameObject exitBox;
    public void ButtonRetryClick()
    {
        Modules.ResetDataFree();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void ButtonHomeClick()
    {
        exitBox.SetActive(true);
        Modules.pauseGame = true;
        Modules.ThrowItem(Modules.localMouse - 1, rows, listPick, listLocal);
        Modules.keepItem = false;
    }
    public void ButtonYesClick()
    {
        Modules.timeNowFree = (int)timeGame;
        Modules.scoreNowFree = Modules.scoreScene;
        Modules.SaveTimeNowFree();
        Modules.SaveScoreNowFree();
        StartCoroutine(WaitLoadSceneHome());
    }
    public void ButtonNoClick()
    {
        Modules.ResetDataFree();
        StartCoroutine(WaitLoadSceneHome());
    }
    public void ButtonHideExitBoxClick()
    {
        exitBox.SetActive(false);
        Modules.pauseGame = false;
        Modules.ThrowItem(Modules.localMouse - 1, rows, listPick, listLocal);
        Modules.keepItem = false;
    }
    IEnumerator WaitLoadSceneHome()
    {
        yield return new WaitForSeconds(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    //
    //xu ly nut leaderboard
    public GameObject leaderBoardBox;
    public void ButtonLeaderBoardClick()
    {
        leaderBoardBox.SetActive(true);
        Modules.lbShow = true;
        Modules.pauseGame = true;
        Modules.ThrowItem(Modules.localMouse - 1, rows, listPick, listLocal);
        Modules.keepItem = false;
   }
    // set _barrierTop 
    public GameObject barrierTop;
    //
    //xy ly khi thua
    public GameObject questionSaveBox, infoPlayerBox;
    public void ButtonLostClick()
    {
        questionSaveBox.SetActive(true);
        lostBox.SetActive(false);
    }
    public void ButtonYes_qs()
    {
        infoPlayerBox.SetActive(true);
        questionSaveBox.SetActive(false);
    }
    public void ButtonNo_qs()
    {
        questionSaveBox.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    //

}
[Serializable]
public class Row
{
    public List<GameObject> items;
    public Row(List<GameObject> _items)
    {
        items = _items;
    }
}