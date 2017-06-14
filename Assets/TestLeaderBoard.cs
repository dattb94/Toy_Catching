using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
public class TestLeaderBoard : MonoBehaviour
{
    void Start()
    {
        LoadLeaderFree();
    }
    public static PlayerInfor[] leaderFree;
    public static void SaveLeaderFree()
    {
        string [] _str = new string[10];
        for (int i = 0; i < 10; i++)
        {
            _str[i] = leaderFree[i].namePlayer + "," + leaderFree[i].avatar.ToString() + "," + leaderFree[i].score.ToString();
            PlayerPrefs.SetString("strLeaderFree"+i,EncryptString(_str[i],"key"));
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
                leaderFree[i] = new PlayerInfor("",0,0);
            }
            print(leaderFree[i].namePlayer);
        }
    }
    public static PlayerInfor StringToPlayerInfo(string _str)
    {
        string[] strs = new string[3];
        strs = _str.Split(',');
        return new PlayerInfor(strs[0], Int32.Parse(strs[1]), Int32.Parse(strs[2]));
    }
    public bool CompareWithLeader(int _value)
    {
        for (int i = 0; i < leaderFree.Length; i++)
        {
            if (leaderFree[i].score < _value)
                return true;
        }
        return false;
    }
    public void UpdateLeaderFree(PlayerInfor _new)
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
    }
    //
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
}
