using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modules : MonoBehaviour {
    #region xu ly chung
    public static int countBeChoise = 0;
    public static bool keepItem;
    public static int scoreScene = 0;
    public static int localMouse = 0;
    public static bool isSpawning = false;
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
    public static void PickItem(int _row, List<GameObject> _listPick, Row[] _rows, GameObject _goChoise, Transform[] _listLocal)
    {
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
    #endregion
    #region xu ly phan champain
    public static int scoreTotalCampain;
    public static int indexCampainNow;
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
    }
    public static void ResetCampainData()
    {
        indexCampainNow = 0;
        scoreTotalCampain = 0;
        SaveIndexCampainNow();
        SaveScoreTotalCampain();

    }
    #endregion
}
