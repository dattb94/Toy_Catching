using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
public class Modules : MonoBehaviour {
    #region xu ly chung
    public static int countBeChoise = 0;
    public static bool keepItem;
    public static bool isCanPick;
    public static int scoreScene = 0;
    public static int localMouse = 0;
    public static bool isSpawning = false;
    public static bool pauseGame = false;
    public static int levelFreeNow = 0;
    public static void CalcuItemBeChoise(Transform[] _listLocal)
    {
        int x = 0;
        for (int j = 0; j < _listLocal.Length; j++)
            for (int i = 0; i < _listLocal[j].transform.childCount; i++)
            {
                if (_listLocal[j].transform.GetChild(i).GetComponent<item>())
                    if (_listLocal[j].transform.GetChild(i).GetComponent<item>().beChoise)
                        x += 1;
            }
        countBeChoise = x;
        if (x > 3)
        {
            scoreScene += (x * 50);
        }
    }
    public void SetIsCanPick()
    {
        StartCoroutine(WaitIsCanPick());
    }
    public static IEnumerator WaitIsCanPick()
    {
        yield return new WaitForSeconds(0.2f);
        isCanPick = true;
    }
    public static void PickItem(int _row, List<GameObject> _listPick, Row[] _rows, GameObject _goChoise, Transform[] _listLocal)
    {
        if (!isCanPick)
            return;
        _listPick.Clear();
        int x = 0;
        if (_rows[_row].items[0] != null)
        {
            if (_rows[_row].items[0].GetComponent<item>().isIron)
                return;
        }
        for (int i = 0; i < _rows[_row].items.Count - 1; i++)
        {
            if (_rows[_row].items[i].GetComponent<item>().type == _rows[_row].items[i + 1].GetComponent<item>().type)
            {
                x += 1;
            }
            else break;
        }
        GameObject _type = _type = _rows[_row].items[0];
        GameObject[] a = new GameObject[x + 1];
        for (int i = 0; i < x + 1; i++)
        {
            a[i] = _rows[_row].items[i];
        }
        for (int i = 0; i < a.Length; i++)
        {
            Destroy(a[i]);
        }
        for (int i = 0; i < x + 1; i++)
        {
            SpawnImage(_type.GetComponent<item>().type, new Vector3(_goChoise.transform.position.x,
                                                                    _goChoise.transform.position.y + DistanceItems() * i,
                                                                    _goChoise.transform.position.z));
            _listPick.Add(Item(_type.GetComponent<item>().type));
        }
        keepItem = true;
        Row(_row, _rows, _listLocal);
    }
    public static void ThrowItem(int _row, Row[] _Rrows, List<GameObject> _listPick, Transform[] _listLocal)
    {
        Modules.Row(_row, _Rrows, _listLocal);
        for (int i = 0; i < _listPick.Count; i++)
        {
            if (_Rrows[_row].items.Count >= 1)
            {
                GameObject item = (GameObject)Instantiate(_listPick[i],
                                                     new Vector3(_listLocal[_row].transform.position.x,
                                                     LocalBarManager.A.transform.position.y + DistanceItems() - DistanceItems() - i * DistanceItems(), 0),
                                                     new Quaternion(0, 0, 0, 0));

                item.GetComponent<item>().beChoise = true;
                item.transform.SetParent(_listLocal[_row]);
            }
            else {
                GameObject item = (GameObject)Instantiate(_listPick[i],
                                                    new Vector3(_listLocal[_row].transform.position.x,
                                                    LocalBarManager.A.transform.position.y + DistanceItems() - DistanceItems() - i * DistanceItems(), 0),
                                                    new Quaternion(0, 0, 0, 0));
                item.GetComponent<item>().beChoise = true;
                item.transform.SetParent(_listLocal[_row]);
            }
        }
        Modules.Row(_row, _Rrows, _listLocal);

        if (_listPick[0].transform.GetComponent<item>().isBoom)
        {
            DestroyRow(_row, _listLocal);
        }
        Modules.keepItem = false;
    }
    public static void SetChoisePosition(GameObject _goChoise, Transform[] _listLocal)
    {
        switch (Modules.localMouse)
        {
            case 1: _goChoise.transform.position = new Vector3(_listLocal[0].position.x, _goChoise.transform.position.y, 0); break;
            case 2: _goChoise.transform.position = new Vector3(_listLocal[1].position.x, _goChoise.transform.position.y, 0); break;
            case 3: _goChoise.transform.position = new Vector3(_listLocal[2].position.x, _goChoise.transform.position.y, 0); break;
            case 4: _goChoise.transform.position = new Vector3(_listLocal[3].position.x, _goChoise.transform.position.y, 0); break;
            case 5: _goChoise.transform.position = new Vector3(_listLocal[4].position.x, _goChoise.transform.position.y, 0); break;
            case 6: _goChoise.transform.position = new Vector3(_listLocal[5].position.x, _goChoise.transform.position.y, 0); break;
            case 7: _goChoise.transform.position = new Vector3(_listLocal[6].position.x, _goChoise.transform.position.y, 0); break;
            case 8: _goChoise.transform.position = new Vector3(_listLocal[7].position.x, _goChoise.transform.position.y, 0); break;
            case 9: _goChoise.transform.position = new Vector3(_listLocal[8].position.x, _goChoise.transform.position.y, 0); break;
            default: break;
        }
    }
    public static float DistanceItems()
    {
        return Vector3.Distance(GameObject.Find("p1").transform.position, GameObject.Find("p2").transform.position);
    }
    public static void Spawn(Transform[] _listLocal)
    {
        Modules.isSpawning = true;
        for (int i = 0; i < _listLocal.Length; i++)
        {
            GameObject go = null;
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Campaign")
                go = RdItemsCampain();
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "demo")
                go = RdItemsFree();
            GameObject item = Instantiate(go, _listLocal[i].position, go.transform.rotation) as GameObject;
            switch (i)
            {
                case 0: item.transform.SetParent(_listLocal[0]); break;
                case 1: item.transform.SetParent(_listLocal[1]); break;
                case 2: item.transform.SetParent(_listLocal[2]); break;
                case 3: item.transform.SetParent(_listLocal[3]); break;
                case 4: item.transform.SetParent(_listLocal[4]); break;
                case 5: item.transform.SetParent(_listLocal[5]); break;
                case 6: item.transform.SetParent(_listLocal[6]); break;
                case 7: item.transform.SetParent(_listLocal[7]); break;
                case 8: item.transform.SetParent(_listLocal[8]); break;
            }
        }
    }
    static GameObject RdItemsFree()
    {
        int x = 0;
        int boom = 2;
        float normal = 100 / 3;
        int iron = 1;
        switch (levelFreeNow)
        {
            case 1: iron = 0; normal = (100 - boom - iron) / 3; break;
            case 2: iron = 0; normal = (100 - boom - iron) / 3; break;
            case 3: iron = 1; boom = 3; normal = (100 - boom - iron - iron) / 4; break;
            case 4: iron = 2; boom = 4; normal = (100 - boom - iron) / 4; break;
            case 5: iron = 2; boom = 5; normal = (100 - boom - iron) / 4; break;
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
    static GameObject RdItemsCampain()
    {
        print("campain");
        int x = 0;
        float normal;
        switch (Modules.indexCampainNow)
        {
            case 0: normal = (100 / 3); break;
            default: normal = (100 - Modules.xxBoom - Modules.xxIron) / 4; ; break;
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
        else if (x > normal * 4 && x <= 100 - Modules.xxBoom - Modules.xxIron)
            return Modules.Item(5);
        else if (x >= 100 - Modules.xxBoom)
            return Modules.Item(6);
        else {
            return Modules.Item(1);
        }
    }
    public static GameObject Item(int __type)
    {
        if (__type == 1) return Resources.Load<GameObject>("item/item1");
        if (__type == 2) return Resources.Load<GameObject>("item/item2");
        if (__type == 3) return Resources.Load<GameObject>("item/item3");
        if (__type == 4) return Resources.Load<GameObject>("item/item4");
        if (__type == 5) return Resources.Load<GameObject>("item/item5");
        if (__type == 6) return Resources.Load<GameObject>("item/item6");
        else return null;
    }
    public static void SpawnImage(int _type, Vector3 _pos)
    {
        switch (_type)
        {
            case 1:
                {
                    GameObject image = (GameObject)Instantiate(Resources.Load<GameObject>("item/item1_image"), _pos, new Quaternion(0, 0, 0, 0));
                    image.name = "ItemImage";
                    break;
                }
            case 2:
                {
                    GameObject image = (GameObject)Instantiate(Resources.Load<GameObject>("item/item2_image"), _pos, new Quaternion(0, 0, 0, 0));
                    image.name = "ItemImage";
                    break;
                }
            case 3:
                {
                    GameObject image = (GameObject)Instantiate(Resources.Load<GameObject>("item/item3_image"), _pos, new Quaternion(0, 0, 0, 0));
                    image.name = "ItemImage";
                    break;
                }
            case 4:
                {
                    GameObject image = (GameObject)Instantiate(Resources.Load<GameObject>("item/item4_image"), _pos, new Quaternion(0, 0, 0, 0));
                    image.name = "ItemImage";
                    break;
                }
            case 6:
                {
                    GameObject image = (GameObject)Instantiate(Resources.Load<GameObject>("item/item6_image"), _pos, new Quaternion(0, 0, 0, 0));
                    image.name = "ItemImage";
                    break;
                }
        }
    }
    public static void Row(int _row, Row[] _rows, Transform[] _listLocal)
    {
        _rows[_row].items.Clear();
        for (int i = 0; i < _listLocal[_row].transform.childCount; i++)
        {
            _rows[_row].items.Add(_listLocal[_row].transform.GetChild(i).gameObject);
        }
        ArrangeRow(_rows[_row].items);
    }
    public static void ArrangeRow(List<GameObject> _list)
    {
        for (int i = 0; i < _list.Count - 1; i++)
            for (int j = i + 1; j < _list.Count; j++)
            {
                if (_list[i].transform.position.y > _list[j].transform.position.y)
                {
                    GameObject x = _list[i];
                    _list[i] = _list[j];
                    _list[j] = x;
                }
            }
    }
    public static void DestroyRow(int _row, Transform[] _listLocal)
    {
        for (int i = 0; i < _listLocal[_row].transform.childCount; i++)
        {
            _listLocal[_row].transform.GetChild(i).gameObject.GetComponent<item>()._DesTroy();
        }
    }
    public static void SetBarrierTop(GameObject _barrierTop)
    {
        GameObject p1 = GameObject.Find("p1");
        _barrierTop.transform.localScale = new Vector3(_barrierTop.transform.localScale.x, p1.transform.localScale.x,
            _barrierTop.transform.localScale.z);
        _barrierTop.transform.position = new Vector3(_barrierTop.transform.position.x, p1.transform.position.y + Modules.DistanceItems(),
            _barrierTop.transform.position.z);
    }
    // xu ly animation box
    public static bool lbShow = false;// ani leader board
    //
    #endregion
    #region xu ly phan champain
    public static int scoreTotalCampain;
    public static int indexCampainNow;
    public static int level
    {
        get
        {
            LoadDataCampain();
            return indexCampainNow + 1;
        }
    }
    public static int xxBoom, xxIron, scoreNeed;
    public static void SaveScoreTotalCampain()
    {
        PlayerPrefs.SetInt("scoreTotalCampain", scoreTotalCampain);
        PlayerPrefs.Save();
    }
    public static void SaveIndexCampainNow()
    {
        PlayerPrefs.SetInt("indexCampainNow", indexCampainNow);
        PlayerPrefs.Save();
    }
    public static void LoadDataCampain()
    {
        scoreTotalCampain = PlayerPrefs.GetInt("scoreTotalCampain");
        indexCampainNow = PlayerPrefs.GetInt("indexCampainNow");
        switch (indexCampainNow)
        {
            case 0:
                {
                    xxBoom = 0;
                    xxIron = 0;
                    break;
                }
            case 1:
                {
                    xxBoom = 0;
                    xxIron = 0;
                    break;
                }
            case 2:
                {
                    xxBoom = 3;
                    xxIron = 0;
                    break;
                }
            case 3:
                {
                    xxBoom = 3;
                    xxIron = 10;

                    break;
                }
            case 4:
                {
                    xxBoom = 3;
                    xxIron = 10;
                    break;
                }
            default:
                {
                    xxBoom = 3;
                    xxIron = 10;
                    break;
                }
        }
        scoreNeed = 20000 + (indexCampainNow + 1) * 5000;
    }
    public static void ResetCampainData()
    {
        indexCampainNow = 0;
        scoreTotalCampain = 0;
        SaveIndexCampainNow();
        SaveScoreTotalCampain();

    }
    #endregion
    #region xy ly phan free
    public static float timeNowFree;
    public static int scoreNowFree;
    public static void SaveTimeNowFree()
    {
        PlayerPrefs.SetFloat("timeNowFree", timeNowFree);
        PlayerPrefs.Save();
    }
    public static void SaveScoreNowFree()
    {
        PlayerPrefs.SetInt("scoreNowFree", scoreNowFree);
        PlayerPrefs.Save();
    }
    public static void LoadDataFree()
    {
        timeNowFree = PlayerPrefs.GetFloat("timeNowFree");
        scoreNowFree = PlayerPrefs.GetInt("scoreNowFree");
    }
    public static void ResetDataFree()
    {
        timeNowFree = 1f;
        scoreNowFree = 0;
        SaveScoreNowFree();
        SaveTimeNowFree();
    }
    #endregion
    #region xu ly leader board
    // Xu ly phan Set player infor
    public static string namePlayer;
    public static Sprite avatar;
    public static int indexAvatar = 100;
    public static Sprite GetAvatar(int _index)
    {
        if (_index == 0)
            return Resources.Load<Sprite>("Avatar/0");
        else if (_index == 1)
            return Resources.Load<Sprite>("Avatar/1");
        else if (_index == 2)
            return Resources.Load<Sprite>("Avatar/2");
        else if (_index == 3)
            return Resources.Load<Sprite>("Avatar/3");
        else if (_index == 4)
            return Resources.Load<Sprite>("Avatar/4");
        else if (_index == 5)
            return Resources.Load<Sprite>("Avatar/5");
        else if (_index == 6)
            return Resources.Load<Sprite>("Avatar/6");
        else if (_index == 7)
            return Resources.Load<Sprite>("Avatar/7");
        else if (_index == 8)
            return Resources.Load<Sprite>("Avatar/8");
        else if (_index == 9)
            return Resources.Load<Sprite>("Avatar/9");
        else if (_index == 10)
            return Resources.Load<Sprite>("Avatar/10");
        else if (_index == 11)
            return Resources.Load<Sprite>("Avatar/11");
        else return null;
    }
    public static void ResetLeaderBoard()
    {
        leaderFree.Clear();
        leaderCampain.Clear();
        SaveLeaderCampain();
        SaveLeaderFree();
    }
    //
    //xu ly phan save and load du lieu leader free
    public static List<PlayerInfor> leaderFree = new List<PlayerInfor>();
    public static void SaveLeaderFree()
    {
        string dataPath = "G:/Du An Unity/LeaderBoardOffline/Assets/leaderFree.json";
        if (!File.Exists(dataPath))
        {
            File.Create(dataPath);
            File.WriteAllText(dataPath, JsonMapper.ToJson(leaderFree));
        }
        else
            File.WriteAllText(dataPath, JsonMapper.ToJson(leaderFree));
    }
    public static void LoadLeaderBoardFree()
    {
        //Lay du lieu tu file json ve
        leaderFree.Clear();
        string dataPath = "G:/Du An Unity/LeaderBoardOffline/Assets/leaderFree.json";
        string jsonString = File.ReadAllText(dataPath);
        JsonData json = JsonMapper.ToObject(jsonString);
        for (int i = 0; i < json.Count; i++)
        {
            {
                leaderFree.Add(new PlayerInfor((string)json[i]["namePlayer"], (int)json[i]["avatar"], (int)json[i]["score"]));
            }
        }
        // Sap xep leader theo score
        for (int i = 0; i < leaderFree.Count - 1; i++)
            for (int j = i + 1; j < leaderFree.Count; j++)
            {
                if (leaderFree[i].score < leaderFree[j].score)
                {
                    PlayerInfor x = leaderFree[i];
                    leaderFree[i] = leaderFree[j];
                    leaderFree[j] = x;
                }
            }
        print(jsonString);
        print(leaderFree.Count);
    }
    public static void AddNewDataToLeaderFree(PlayerInfor _newData)
    {
        LoadDataFree();
        leaderFree.Add(_newData);
        SaveLeaderFree();
        print("add");
    }
    //
    // Xu ly phan leader board campain
    public static List<PlayerInfor> leaderCampain = new List<PlayerInfor>();
    public static void SaveLeaderCampain()
    {
        string dataPath = "G:/Du An Unity/LeaderBoardOffline/Assets/leaderCampain.json";
        if (!File.Exists(dataPath))
        {
            File.Create(dataPath);
            File.WriteAllText(dataPath, JsonMapper.ToJson(leaderCampain));
        }
        else
            File.WriteAllText(dataPath, JsonMapper.ToJson(leaderCampain));
    }
    public static void LoadLeaderBoardCampain()
    {
        //Lay du lieu tu file json ve
        leaderCampain.Clear();
        string dataPath = "G:/Du An Unity/LeaderBoardOffline/Assets/leaderCampain.json";
        string jsonString = File.ReadAllText(dataPath);
        JsonData json = JsonMapper.ToObject(jsonString);
        for (int i = 0; i < json.Count; i++)
        {
            {
                leaderCampain.Add(new PlayerInfor((string)json[i]["namePlayer"], (int)json[i]["avatar"], (int)json[i]["score"], (int)json[i]["level"]));
            }
        }
        // Sap xep leader theo score
        for (int i = 0; i < leaderCampain.Count - 1; i++)
            for (int j = i + 1; j < leaderCampain.Count; j++)
            {
                if (leaderCampain[i].level < leaderCampain[j].level)
                {
                    PlayerInfor x = leaderCampain[i];
                    leaderCampain[i] = leaderCampain[j];
                    leaderCampain[j] = x;
                }
            }
    }
    public static void AddNewDataToLeaderCampain(PlayerInfor _newData)
    {
        LoadLeaderBoardCampain();
        leaderCampain.Add(_newData);
        SaveLeaderCampain();
    }
    //
    #endregion
    #region xu ly am thanh
    public static float volume = 1;
    public static void SaveVolum()
    {
        PlayerPrefs.SetFloat("volume",volume);
        PlayerPrefs.Save();
    }
    public static void LoadAudio()
    {
        volume = PlayerPrefs.GetFloat("volume");
    }
    #endregion
}
public class PlayerInfor
{
    public string namePlayer;
    public int avatar;
    public int score;
    public int level;

    public PlayerInfor(string _name, int _avatar, int _score)// khai bao phan free
    {
        namePlayer = _name;
        avatar = _avatar;
        score = _score;
    }
    public PlayerInfor(string _name, int _avatar, int _score, int _level)// khai bao phan campain
    {
        namePlayer = _name;
        avatar = _avatar;
        score = _score;
        level = _level;
    }
}