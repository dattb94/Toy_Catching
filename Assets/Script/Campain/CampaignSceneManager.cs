using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CampaignSceneManager : MonoBehaviour {
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
    int levelGame = 1, xxBoom = 0, xxIron = 0, scoreNeed;
    int timePlay, timeMax = 10;
    bool win = false;
    int maxCampain;
    public Transform parentCampainStack;
    void Start()
    {
        Modules.isCanPick = true;
        maxCampain = parentCampainStack.childCount;
        SetBarrierTop();
        Modules.LoadDataCampain();
        Modules.keepItem = false;
        win = false;
        Modules.scoreScene = 0;
        levelGame = Modules.indexCampainNow;
        InitValueStart();
        beLost = false;
        timeSpawn = TimeSpawn - 1;
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i] = new Row(new List<GameObject>());
        }
    }
    void InitValueStart()
    {
        switch (levelGame)
        {
            case 0: {
                    xxBoom = 0;
                    xxIron = 0;

                    scoreNeed = 20000;

                    break;
                }
            case 1:
                {
                    xxBoom = 0;
                    xxIron = 0;
                    scoreNeed = 25000;
                    break;
                }
            case 2:
                {
                    xxBoom = 3;
                    xxIron = 0;
                    scoreNeed = 35000;

                    break;
                }
            case 3:
                {
                    xxBoom = 3;
                    xxIron = 10;
                    scoreNeed = 45000;

                    break;
                }
            case 4:
                {
                    xxBoom = 3;
                    xxIron = 10;
                    scoreNeed = 60000;

                    break;
                }
            default:
                {
                    xxBoom = 3;
                    xxIron = 10;
                    scoreNeed = 70000;

                    break;
                }
        }
    }
    public GameObject CamPainContain, StartCampainContain;
    public void ButtonHideWinBox()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    public GameObject WinBox, LostBox;
    public Text textScoreScene, textScoreTotal, textLevelWin;
    void Win()
    {
        Modules.LoadDataCampain();
        win = true;
        WinBox.SetActive(true);
        textScoreScene.text = "Score: " + Modules.scoreScene;
        textLevelWin.text = "Level: " + Modules.indexCampainNow;
        Modules.scoreTotalCampain += Modules.scoreScene;
        Modules.SaveScoreTotalCampain();
        textScoreTotal.text = "Score total: " + Modules.scoreTotalCampain;
        Modules.indexCampainNow += 1;
        Modules.SaveIndexCampainNow();
    }
    void Update()
    {
        if (Modules.pauseGame)
            return;
        SetDataStateBar();
        if (Modules.scoreScene >= scoreNeed && !win)
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
            Spawn();
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
    public Image imgScoreNeed, imgTimer, imgTimeSpawn;
    public Text textTimer, textScoreNeed, textLevel, textScore, textTotalSCore;
    void SetDataStateBar()
    {
        imgScoreNeed.fillAmount = (float)Modules.scoreScene / scoreNeed;
        //imgTimer.fillAmount = (float)scoreNeed / Modules.scoreScene;
        imgTimeSpawn.fillAmount = (float)timeSpawn / TimeSpawn;
        //print((float)timeSpawn / TimeSpawn);
        //textScoreNeed.text = (int)(((float)Modules.scoreScene / scoreNeed) * 100) + "%";
        textScoreNeed.text = Modules.scoreScene +"/"+ scoreNeed;
        textLevel.text = "Level: " + levelGame;
        textScore.text = "Score: " + Modules.scoreScene;
        textTotalSCore.text = "Total score: " + Modules.scoreTotalCampain;
    }
    //
    void Spawn()
    {
        Modules.isSpawning = true;
        for (int i = 0; i < listLocal.Length; i++)
        {
            GameObject item = Instantiate(RdItemsLv1(), listLocal[i].position, RdItemsLv1().transform.rotation) as GameObject;
            switch (i)
            {
                case 0: item.transform.SetParent(listLocal[0]); break;
                case 1: item.transform.SetParent(listLocal[1]); break;
                case 2: item.transform.SetParent(listLocal[2]); break;
                case 3: item.transform.SetParent(listLocal[3]); break;
                case 4: item.transform.SetParent(listLocal[4]); break;
                case 5: item.transform.SetParent(listLocal[5]); break;
                case 6: item.transform.SetParent(listLocal[6]); break;
                case 7: item.transform.SetParent(listLocal[7]); break;
                case 8: item.transform.SetParent(listLocal[8]); break;
            }
        }
    }
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
    IEnumerator IESpawn()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < listLocal.Length; i++)
        {
            GameObject item = Instantiate(RdItemsLv1(), listLocal[i].position, RdItemsLv1().transform.rotation) as GameObject;
            switch (i)
            {
                case 0: item.transform.SetParent(listLocal[0]); break;
                case 1: item.transform.SetParent(listLocal[1]); break;
                case 2: item.transform.SetParent(listLocal[2]); break;
                case 3: item.transform.SetParent(listLocal[3]); break;
                case 4: item.transform.SetParent(listLocal[4]); break;
                case 5: item.transform.SetParent(listLocal[5]); break;
                case 6: item.transform.SetParent(listLocal[6]); break;
                case 7: item.transform.SetParent(listLocal[7]); break;
                case 8: item.transform.SetParent(listLocal[8]); break;
            }
        }
    }
    GameObject RdItemsLv1()
    {
        int x = 0;
        float normal;
        switch (levelGame)
        {
            case 0: normal = (100 / 3); break;
            default: normal = (100 - xxBoom - xxIron) / 4; ; break;
        }
        x = UnityEngine.Random.Range(0, 100);
        if (x >= 0 && x <= normal)
            return Modules.Item(1);
        else if (x > normal && x <= normal * 2)
            return Modules.Item(2);
        else if (x > normal * 2 && x <= normal * 3)
            return Modules.Item(3);
        else if (x > normal * 3 && x <= normal * 4)
            return Modules.Item(4);
        else if (x > normal * 4 && x <= 100 - xxBoom - xxIron)
            return Modules.Item(5);
        else if (x >= 100 - xxBoom)
            return Modules.Item(6);
        else {
            return Modules.Item(1);
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
    public GameObject barrierTop, p1;
    void SetBarrierTop()
    {
        barrierTop.transform.localScale = new Vector3(barrierTop.transform.localScale.x, p1.transform.localScale.x, 
            barrierTop.transform.localScale.z);
        barrierTop.transform.position = new Vector3(barrierTop.transform.position.x, p1. transform.position.y+Modules.DistanceItems(),
            barrierTop.transform.position.z);
    }
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
