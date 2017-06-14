using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestStartCampain : MonoBehaviour {
    public int lvMin, lvMax, lvNow;
    void Start()
    {
        SetDataCanvas();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Reset();
    }
    public GameObject stackparent, buttonNew, buttonPre, startCampainContain, campainContain;
    public Text textstarNow, textstarTarget, textCampainNow;
    void SetDataCanvas()
    {
        LoadDataLevel();
        //set data text
        int ii = campainNow + 1;
        textCampainNow.text = "Campain: " + ii;

        int startNow=0;
        for (int i = 0; i < _level; i++)
        {
            startNow += _star[i];
        }

        textstarNow.text = "star: " + startNow;
        textstarTarget.text = "target star: " + GetStaTarget();
        //stdata button
        if (campainNow > 0)
        {
            buttonPre.GetComponent<Image>().color = Color.white;
            buttonPre.GetComponent<Button>().enabled = true;
        }
        print(campainNow+" "+(int)_level/6);
        if (campainNow < (int)_level / 6)
        {
            buttonNew.GetComponent<Image>().color = Color.white;
            buttonNew.GetComponent<Button>().enabled = true;
        }
        //set data stackParent
        Transform parent = stackparent.transform;
        for (int i = 0; i < parent.childCount; i++)
        {
            int levelStack = campainNow * 6 + (i);
            parent.GetChild(i).FindChild("textLevel").GetComponent<Text>().text = (levelStack + 1) + "";
            //print(campainNow * 6 + (i)+"   "+i);
            if (levelStack < _level)
            {
                parent.GetChild(i).FindChild("imgcenter").GetComponent<Image>().color=Color.green;
                parent.GetChild(i).FindChild("checkmask").gameObject.SetActive(true);
                parent.GetChild(i).FindChild("Button").gameObject.SetActive(true);
                parent.GetChild(i).FindChild("bgstar").gameObject.SetActive(true);

                //print(levelStack + " " + _star[levelStack]);
                for (int j = 0; j < _star[levelStack]; j++)
                {
                    parent.GetChild(i).FindChild("bgstar").GetChild(j).gameObject.SetActive(true);
                }
            }
            if (levelStack == _level)
            {
                parent.GetChild(i).FindChild("imgcenter").GetComponent<Image>().color = Color.white;
                //parent.GetChild(i).FindChild("textLevel").GetComponent<Text>().text = (levelStack + 1) + "";
                parent.GetChild(i).FindChild("checkmask").gameObject.SetActive(false);
                parent.GetChild(i).FindChild("Button").gameObject.SetActive(true);
            }
            if (levelStack > _level)
            {
                parent.GetChild(i).FindChild("imgcenter").GetComponent<Image>().color = Color.grey;
                //parent.GetChild(i).FindChild("textLevel").GetComponent<Text>().text = (levelStack + 1) + "";
                parent.GetChild(i).FindChild("checkmask").gameObject.SetActive(false);
                parent.GetChild(i).FindChild("Button").gameObject.SetActive(false);
            }
        }

    }
    int GetStaTarget()
    {
        return (campainNow + 1) * 20;
    }
    public void ButtonStackClick(int _index)
    {
        _indexer = campainNow * 6 + _index;
        startCampainContain.SetActive(false);
        campainContain.SetActive(true);
    }
    public void ButtonNextClick()
    {
        LoadDataLevel();
        campainNow += 1;
        SaveCampainNow();
        Application.LoadLevel(Application.loadedLevelName);
    }
    public void ButtonPreClick()
    {
        LoadDataLevel();
        campainNow -= 1;
        SaveCampainNow();
        Application.LoadLevel(Application.loadedLevelName);
    }
    //luu tru thong tin
    public static int _level;//nhiem vu hien co cao nhat
    public static void Save_Level()
    {
        PlayerPrefs.SetString("_level", TestLeaderBoard.EncryptString(_level.ToString(), "key"));
        PlayerPrefs.Save();
    }

    public static int campainNow=0;//capain hien tai
    public static void SaveCampainNow()
    {
        PlayerPrefs.SetString("campainNow", TestLeaderBoard.EncryptString(campainNow.ToString(), "key"));
        PlayerPrefs.Save();
    }

    public static int _indexer;//nhiem vu dang lam
    public static List<int> _star= new List<int>();// danh sach star hien co moi level
    public static void Save_Star()
    {
        for (int i = 0; i < _star.Count; i++)
        {
            PlayerPrefs.DeleteKey("_star" + i);
            PlayerPrefs.SetString("_star" + i, TestLeaderBoard.EncryptString(_star[i].ToString(), "key"));
            PlayerPrefs.Save();
        }
    }

    public static void LoadDataLevel()
    {
        if (PlayerPrefs.HasKey("_level"))
            _level = System.Int32.Parse(TestLeaderBoard.DecryptString(PlayerPrefs.GetString("_level"), "key"));
        else _level = 0;
        if (PlayerPrefs.HasKey("campainNow"))
            campainNow = System.Int32.Parse(TestLeaderBoard.DecryptString(PlayerPrefs.GetString("campainNow"), "key"));
        else campainNow = 0;
        
        _star = new List<int>();
        _star.Clear();
        for (int i = 0; i < _level; i++)
        {
            if (PlayerPrefs.HasKey("_star" + i))
            {
                _star.Add(System.Int32.Parse(TestLeaderBoard.DecryptString(PlayerPrefs.GetString("_star" + i), "key")));
                //print(i+"   "+_star[i]);
            }
            else _star.Add(0);
        }
    }

    public void AddNewLevel()
    {
        //_level
    }
    public static void Reset()
    {
        campainNow = 0;
        _level = 0;
        _star.Clear();
        SaveCampainNow();
        Save_Level();
        Save_Star();
        Application.LoadLevel(Application.loadedLevelName);
    }

}
