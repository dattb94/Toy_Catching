using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
using System;
public class Moduless : MonoBehaviour {
    #region xu ly phan save and load du lieu leader free
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
    }
    public static void AddNewDataToLeaderFree(PlayerInfor _newData)
    {
        leaderFree.Add(_newData);
        SaveLeaderFree();
    }
    public static Sprite GetAvatar(Sprite[] _avatars, int _type)
    {
        for (int i = 0; i < _avatars.Length; i++)
        {
            if (_type == i)
                return _avatars[i];
        }
        return null;
    }
    #endregion
    #region Xu ly phan leader board campain
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
        leaderCampain.Add(_newData);
        SaveLeaderCampain();
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