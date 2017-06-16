using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
using System;
using System.Security.Cryptography;
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
            {
                PlayAudio("ironItem",0.3f);
                return;
            }
            else PlayAudio("pickItem", 0.5f);
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
        PlayAudio("throwitem", 0.3f);
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
        //if(!GameObject.Find("auspawnItem"))
            Modules.PlayAudio("spawnItem",0.2f);
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
    public static bool lbShow = false;// animation leader board
    //

    #endregion
    #region xu ly phan champain
    public static int scoreTotalCampain;//tong diem
    //phan moi
    public static int levelCampain;//nhiem vu hien co cao nhat
    public static void SavelevelCampain()
    {
        PlayerPrefs.SetString("levelCampain", EncryptString(levelCampain.ToString(), "key"));
        PlayerPrefs.Save();
    }

    public static int starBonus;// sao thuong them
    public static void SaveStarBonus()
    {
        PlayerPrefs.SetString("starBonus", EncryptString(starBonus.ToString(), "key"));
        PlayerPrefs.Save();
    }

    public static int campainNow = 0;//capain hien tai
    public static void SaveCampainNow()
    {
        PlayerPrefs.SetString("campainNow", EncryptString(campainNow.ToString(), "key"));
        PlayerPrefs.Save();
    }

    public static int indexCampainNow;//nhiem vu dang lam

    public static List<int> starStack = new List<int>();// danh sach star hien co moi level
    public static void SavestarStack()
    {
        for (int i = 0; i < starStack.Count; i++)
        {
            PlayerPrefs.DeleteKey("starStack" + i);
            PlayerPrefs.SetString("starStack" + i, EncryptString(starStack[i].ToString(), "key"));
            PlayerPrefs.Save();
        }
    }

    public static void LoadDataLevel()
    {
        if (PlayerPrefs.HasKey("levelCampain"))//load level campain
            levelCampain = System.Int32.Parse(DecryptString(PlayerPrefs.GetString("levelCampain"), "key"));

        else levelCampain = 0;
        if (PlayerPrefs.HasKey("campainNow"))// load index campain now
            campainNow = System.Int32.Parse(DecryptString(PlayerPrefs.GetString("campainNow"), "key"));
        else campainNow = 0;

        if (PlayerPrefs.HasKey("starBonus"))//load level campain
            starBonus = System.Int32.Parse(DecryptString(PlayerPrefs.GetString("starBonus"), "key"));

        else starBonus = 0;

        //Load star levels
        starStack = new List<int>();
        starStack.Clear();
        for (int i = 0; i < levelCampain; i++)
        {
            if (PlayerPrefs.HasKey("starStack" + i))
            {
                starStack.Add(System.Int32.Parse(DecryptString(PlayerPrefs.GetString("starStack" + i), "key")));
            }
            else starStack.Add(0);
        }
    }
    //
    public static int xxBoom, xxIron, scoreNeed;
    public static void SaveScoreTotalCampain()
    {
        PlayerPrefs.SetInt("scoreTotalCampain", scoreTotalCampain);
        PlayerPrefs.Save();
    }
    public static void LoadDataCampain()
    {
        scoreTotalCampain = PlayerPrefs.GetInt("scoreTotalCampain");
        #region
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
        #endregion
        scoreNeed = 20000 + (indexCampainNow + 1) * 5000;
        //code moi
        if (PlayerPrefs.HasKey("levelCampain"))
            levelCampain = System.Int32.Parse(DecryptString(PlayerPrefs.GetString("levelCampain"), "key"));
        else levelCampain = 0;
        if (PlayerPrefs.HasKey("campainNow"))
            campainNow = System.Int32.Parse(DecryptString(PlayerPrefs.GetString("campainNow"), "key"));
        else campainNow = 0;
        starStack = new List<int>();
        starStack.Clear();
        for (int i = 0; i < levelCampain; i++)
        {
            if (PlayerPrefs.HasKey("starStack" + i))
            {
                starStack.Add(System.Int32.Parse(DecryptString(PlayerPrefs.GetString("starStack" + i), "key")));
            }
            else starStack.Add(0);
        }
    }
    public static void ResetCampainData()
    {
        scoreTotalCampain = 0;
        SaveScoreTotalCampain();
        campainNow = 0;
        levelCampain = 0;
        starStack.Clear();
        SaveCampainNow();
        SavelevelCampain();
        SavestarStack();
        Application.LoadLevel(Application.loadedLevelName);

    }
    #endregion
    #region xy ly phan free
    public static float timeNowFree;// tho gian hien tai dang choi
    public static void SaveTimeNowFree()
    {
        PlayerPrefs.SetString("timeNowFree", EncryptString(timeNowFree.ToString(), "key"));
        //PlayerPrefs.SetFloat("timeNowFree", timeNowFree);
        PlayerPrefs.Save();
    }

    public static int scoreNowFree;//score hien tai
    public static void SaveScoreNowFree()
    {
        PlayerPrefs.SetString("scoreNowFree", EncryptString(scoreNowFree.ToString(), "key"));
        //PlayerPrefs.SetInt("scoreNowFree", scoreNowFree);
        PlayerPrefs.Save();
    }

    public static void LoadDataFree()
    {
       // timeNowFree = PlayerPrefs.GetFloat("timeNowFree");
        //scoreNowFree = PlayerPrefs.GetInt("scoreNowFree");

        if (PlayerPrefs.HasKey("timeNowFree"))//load thoi gian da choi
            timeNowFree = System.Int32.Parse(DecryptString(PlayerPrefs.GetString("timeNowFree"), "key"));
        else timeNowFree = 0;

        if (PlayerPrefs.HasKey("scoreNowFree"))//load diem so da choi
            scoreNowFree = System.Int32.Parse(DecryptString(PlayerPrefs.GetString("scoreNowFree"), "key"));
        else scoreNowFree = 0;

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
        leaderFree= new PlayerInfor[10];
        for (int i = 0; i < 10; i++)
            leaderFree[i] = new PlayerInfor("", 0, 0);
        SaveLeaderFree();
    }
    //

    //xu ly phan save and load du lieu leader free
    public static PlayerInfor[] leaderFree;//danh sach xep hang
    public static void SaveLeaderFree()
    {
        string[] _str = new string[10];
        for (int i = 0; i < 10; i++)
        {
            _str[i] = leaderFree[i].namePlayer + "," + leaderFree[i].avatar.ToString() + "," + leaderFree[i].score.ToString();
            PlayerPrefs.SetString("strLeaderFree" + i, EncryptString(_str[i], "key"));
        }
    }

    public static void LoadLeaderFree()
    {
        leaderFree = new PlayerInfor[10];
        string[] _str = new string[10];
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey("strLeaderFree" + i))
            {
                _str[i] = DecryptString(PlayerPrefs.GetString("strLeaderFree" + i), "key");
                leaderFree[i] = StringToPlayerInfo(_str[i]);
            }
            else
            {
                leaderFree[i] = new PlayerInfor("", 0, 0);
            }
        }
    }
    public static PlayerInfor StringToPlayerInfo(string _str)
    {
        string[] strs = new string[3];
        strs = _str.Split(',');
        return new PlayerInfor(strs[0], Int32.Parse(strs[1]), Int32.Parse(strs[2]));
    }//chuyem doi tu file string sang PlayerInfo
    public static bool CompareWithLeader()
    {
        for (int i = 0; i < leaderFree.Length; i++)
        {
            if (leaderFree[i].score < scoreScene)
                return true;
        }
        return false;
    }//so sanh voi bang xep hang
    public static void UpdateLeaderFree(PlayerInfor _new)
    {
        LoadLeaderFree();
        leaderFree[4] = _new;
        for (int i = 0; i < leaderFree.Length - 1; i++)
            for (int j = i + 1; j < leaderFree.Length; j++)
            {
                if (leaderFree[i].score < leaderFree[j].score)
                {
                    PlayerInfor x = leaderFree[i];
                    leaderFree[i] = leaderFree[j];
                    leaderFree[j] = x;
                }
            }
        SaveLeaderFree();
    }//cap nhat bang xep hang
    //
    #endregion
    #region xu ly am thanh
    public static float volume = 1;//am luong
    public static void SaveVolum()
    {
        PlayerPrefs.SetString("volume", EncryptString(volume.ToString(), "key"));
        //PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }

    public static void PlayAudio(string _nameAudio, float _timePlay)
    {
        GameObject audio = (GameObject)Instantiate(Resources.Load<AudioSource>("AudioSource/"+ _nameAudio).gameObject);
        audio.name = "au"+_nameAudio;
        Destroy(audio,_timePlay);
    }
    public static void LoadAudio()
    {
        if (PlayerPrefs.HasKey("volume"))//load thoi gian da choi
            volume = System.Int32.Parse(DecryptString(PlayerPrefs.GetString("volume"), "key"));
        else volume = 0;
        //volume = PlayerPrefs.GetFloat("volume");
        AudioListener.volume = volume;
    }
    #endregion
    #region mã hoa va giai ma
    public static string EncryptString(string Message, string Passphrase)
    {
        byte[] Results;
        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

        // Buoc 1: Bam chuoi su dung MD5

        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

        // Step 2. Tao doi tuong TripleDESCryptoServiceProvider moi
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

        // Step 3. Cai dat bo ma hoa
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;

        // Step 4. Convert chuoi (Message) thanh dang byte[]
        byte[] DataToEncrypt = UTF8.GetBytes(Message);

        // Step 5. Ma hoa chuoi
        try
        {
            ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
            Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
        }
        finally
        {
            // Xoa moi thong tin ve Triple DES va HashProvider de dam bao an toan
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }

        // Step 6. Tra ve chuoi da ma hoa bang thuat toan Base64
        return Convert.ToBase64String(Results);
    }
    public static string DecryptString(string Message, string Passphrase)
    {
        byte[] Results;
        System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

        // Step 1. Bam chuoi su dung MD5

        MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

        // Step 2. Tao doi tuong TripleDESCryptoServiceProvider moi
        TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

        // Step 3. Cai dat bo giai ma
        TDESAlgorithm.Key = TDESKey;
        TDESAlgorithm.Mode = CipherMode.ECB;
        TDESAlgorithm.Padding = PaddingMode.PKCS7;

        // Step 4. Convert chuoi (Message) thanh dang byte[]
        byte[] DataToDecrypt = Convert.FromBase64String(Message);

        // Step 5. Bat dau giai ma chuoi
        try
        {
            ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
        }
        finally
        {
            // Xoa moi thong tin ve Triple DES va HashProvider de dam bao an toan
            TDESAlgorithm.Clear();
            HashProvider.Clear();
        }

        // Step 6. Tra ve ket qua bang dinh dang UTF8
        return UTF8.GetString(Results);
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
    public PlayerInfor(string _name, int _avatar, int _score, int levelCampain)// khai bao phan campain
    {
        namePlayer = _name;
        avatar = _avatar;
        score = _score;
        level = levelCampain;
    }
}