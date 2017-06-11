using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CampaignSceneManager : MonoBehaviour
{
    public Transform[] listLocal;
    public static float timeSpawn = 0;
    float TimeSpawn = 15;
    public static bool spawn = false;
    public GameObject goChoise;
    public Row[] rows = new Row[9];
    public List<GameObject> listPick;
    float timeGame = 0;
    float loadRows = 0;
    public static bool beLost = false;
    int timePlay, timeMax = 10;
    bool win = false;
    int maxCampain;
    public Transform parentCampainStack;
    void Start()
    {
        Modules.pauseGame = false;
        Modules.isCanPick = true;
        maxCampain = parentCampainStack.childCount;
        Modules.SetBarrierTop(barrierTop);
        Modules.LoadDataCampain();
        Modules.keepItem = false;
        win = false;
        Modules.scoreScene = 0;
        beLost = false;
        timeSpawn = TimeSpawn - 1;
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i] = new Row(new List<GameObject>());
        }
    }
    public GameObject CamPainContain, StartCampainContain;
    public GameObject WinBox, LostBox;
    public Text textScoreScene, textScoreTotal, textLevelWin;
    void Win()
    {
        Modules.LoadDataCampain();
        win = true;
        WinBox.SetActive(true);
        textScoreScene.text = "Score: " + Modules.scoreScene;
        textLevelWin.text = "Level: " + Modules.level;
        Modules.scoreTotalCampain += Modules.scoreScene;
        Modules.SaveScoreTotalCampain();
        textScoreTotal.text = "Score total: " + Modules.scoreTotalCampain;
        Modules.indexCampainNow += 1;
        Modules.SaveIndexCampainNow();
        StartCoroutine(WaitLoadCampainScene());
    }
    IEnumerator WaitLoadCampainScene()
    {
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public void ButtonHideWinBox()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    void Update()
    {
        if (Modules.pauseGame)
            return;
        SetDataStateBar();
        if (Modules.scoreScene >= Modules.scoreNeed && !win)
        {
            Win();
        }
        if (win)
        {
            return;
        }
        CalcuLevel();
        if (beLost)
        {
            LostBox.SetActive(true);
            return;
        }
        if (Modules.keepItem)
        {
            Modules.countBeChoise = 0;
        }
        else {
            Modules.CalcuItemBeChoise(listLocal);
        }
        timeGame += Time.deltaTime;
        timeSpawn += Time.deltaTime;
        if (timeSpawn > TimeSpawn)
        {
            Modules.Spawn(listLocal);
            timeSpawn = 0;
        }
        else Modules.isSpawning = false; ;
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
        if (Input.GetMouseButtonDown(1))
        {
            rows[0].items.Clear();
        }
        loadRows -= Time.deltaTime;
        if (loadRows <= 0)
        {
            for (int i = 0; i < rows.Length; i++)
            {
                Modules.Row(i, rows, listLocal);
            }
        }
    }
    //state bar
    public Image imgScoreNeed, imgTimeSpawn;
    public Text textTimer, textScoreNeed, textLevel, textScore, textTotalSCore;
    void SetDataStateBar()
    {
        imgScoreNeed.fillAmount = (float)Modules.scoreScene / Modules.scoreNeed;
        imgTimeSpawn.fillAmount = (float)timeSpawn / TimeSpawn;
        textScoreNeed.text = Modules.scoreScene + "/" + Modules.scoreNeed;
        textLevel.text = "Level: " + Modules.level;
        textScore.text = "Score: " + Modules.scoreScene;
        textTotalSCore.text = "Total score: " + Modules.scoreTotalCampain;
    }
    //
    void CalcuLevel()
    {
        if (timeGame < 0.5f)
            TimeSpawn = 0.1f;
        if (timeGame < 20 && timeGame >= 0.5f)
        {
            TimeSpawn = 10;
        }
        if (timeGame > 20 && timeGame < 40)
        {
            TimeSpawn = 9;
        }
        if (timeGame > 40 && timeGame < 60)
        {
            TimeSpawn = 8;
        }
        if (timeGame > 60 && timeGame < 80)
        {
            TimeSpawn = 7;
        }
        if (timeGame > 80)
        {
            TimeSpawn = 6;
        }
    }
    public void BtnHome()
    {
        Destroy(GameObject.Find("CamPaignData"));
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void ButtonRetryClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    //xu ly barrier top
    public GameObject barrierTop;
    //
    public void WhenPauseGame()
    {
        Modules.ThrowItem(Modules.localMouse - 1, rows, listPick, listLocal);
        Modules.keepItem = false;
    }
    //xu ly nut leaderboard
    public GameObject leaderBoardBox;
    public void ButtonLeaderBoardClick()
    {
        leaderBoardBox.SetActive(true);
        Modules.pauseGame = true;
        Modules.ThrowItem(Modules.localMouse - 1, rows, listPick, listLocal);
        Modules.keepItem = false;
    }
    //
}
