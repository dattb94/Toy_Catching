using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    public Image imgTime, imgLost;
    public GameObject btnLost;
    public Text txtScore, txtLv, txtTime;
    public Transform[] listLocal;
    public static float timeSpawn = 0;
    float TimeSpawn=10;
    public static bool spawn = false;
    public GameObject goChoise;
    public Row[] rows = new Row[9];
    public List<GameObject> listPick;
    float timeGame = 0;
    int _lv = 0;
    float loadRows = 0;
    public static bool beLost = false;
    void Start()
    {
        Modules.isCanPick = true;
        Modules.pauseGame = false;
        Modules.LoadDataFree();
        Modules.scoreScene = Modules.scoreNowFree;
        Modules.keepItem = false;
        beLost = false;
        timeSpawn = TimeSpawn - 1;
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i] = new Row(new List<GameObject>());
        }
    }
    void Update()
    {
        if (Modules.pauseGame)
            return;
        SetDataButtonPause();
        CalcuLevel();
        if (beLost)
        {
            imgLost.gameObject.SetActive(true);
            btnLost.SetActive(true);
            return;
        }
        else {
            imgLost.gameObject.SetActive(false);
            btnLost.SetActive(false);
        }

        if (Modules.keepItem)
        {
            Modules.countBeChoise = 0;
        }
        else Modules.CalcuItemBeChoise(listLocal);
        timeGame += Time.deltaTime;
        if (timeGame >= 0.5f)
        {
            imgTime.fillAmount = timeSpawn / TimeSpawn;
            txtScore.text = "Score: " + Modules.scoreScene;
            txtLv.text = "Level: " + _lv;
            txtTime.text = "Time:: " + (int)timeGame;
            if (timeGame<0.6f)
            {
                timeGame = Modules.timeNowFree;
            }
        }
        timeSpawn += Time.deltaTime;
        if (timeSpawn > TimeSpawn)
        {
            Spawn();
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
    void Spawn()
    {
        Modules.isSpawning = true;
        StartCoroutine(IESpawn());
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
    void CalcuLevel()
    {
        if (timeGame < 0.5f)
            TimeSpawn = 0.1f;
        if (timeGame < 20&&timeGame>=0.5f)
        {
            _lv = 1;
            TimeSpawn = 10;
        }
        if (timeGame > 20 && timeGame < 40)
        {
            _lv = 2;
            TimeSpawn = 9;
        }
        if (timeGame > 40 && timeGame < 60)
        {
            _lv = 3;
            TimeSpawn = 8;
        }
        if (timeGame > 60 && timeGame < 80)
        {
            _lv = 4;
            TimeSpawn = 7;
        }
        if (timeGame > 80)
        {
            _lv = 5;
            TimeSpawn = 6;
        }
    }
    //xu ly nut' pause game
    public GameObject buttonPause, pauseBox;
    public Sprite sprPause, sprPlay;
    public void SetDataButtonPause()
    {
        if (Modules.pauseGame == true)
        {
            buttonPause.GetComponent<Image>().sprite =sprPlay;
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
        Modules.ThrowItem(Modules.localMouse - 1, rows, listPick, listLocal);
        Modules.keepItem = false;
    }
    GameObject RdItemsLv1()
    {
        int x = 0;
        int boom = 2;
        float normal=100/3;
        int iron = 1;
       
        switch (_lv)
        {
            case 1: iron = 0; normal = (100- boom-iron) / 3; break;
            case 2: iron = 0; normal = (100 - boom - iron) / 3; break;
            case 3: iron = 1; boom = 3; normal = (100 - boom - iron - iron) / 4; break;
            case 4: iron = 2; boom = 4; normal = (100 - boom  - iron) / 4; break;
            case 5: iron = 2; boom = 5; normal = (100 - boom - iron) / 4;  break;
        }
        x = UnityEngine.Random.Range(0, 100);
        if (x >= 0 && x <= normal) return Modules.Item(1);
        if (x > normal && x <= normal * 2) return Modules.Item(2);
        if (x > normal * 2 && x <= normal * 3) return Modules.Item(3);
        if (x > normal * 3 && x <= normal * 4) return Modules.Item(4);
        if (x > normal * 4 && x < 100 - boom) { Debug.Log("x"); return Modules.Item(5); }
        if (x >= 100 - boom) return Modules.Item(6);
        else return null;

    }
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