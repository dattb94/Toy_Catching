using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CampainBox : MonoBehaviour {

    void Start()
    {
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("a",2,45,3));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("ads", 2, 45, 3));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("affdsfg", 2, 45, 3));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("afggfd", 2, 45, 4));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("ahgs", 2, 45, 2));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("aa", 2, 45, 6));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("af", 2, 45, 8));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("ah", 2, 45, 1));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("ar", 2, 45, 3));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("a", 2, 45, 2));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("da", 2, 45, 6));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("af", 2, 45, 44));
        Moduless.AddNewDataToLeaderCampain(new PlayerInfor("ag", 2, 45, 322));
        SetDataContent();
    }
    //xu ly phan du content
    public Image imgCard;
    public GameObject contentBox, cardPlayer;
    void SetDataContent()
    {
        Moduless.LoadLeaderBoardCampain();
        //set height content
        float space = contentBox.GetComponent<VerticalLayoutGroup>().spacing;
        float cardHeigh = imgCard.GetComponent<RectTransform>().sizeDelta.y;
        int count = Moduless.leaderCampain.Count;
        contentBox.GetComponent<RectTransform>().sizeDelta = new Vector2(contentBox.GetComponent<RectTransform>().sizeDelta.x,
            count * (cardHeigh + space));
        //
        //Set data cho cac the? player 
        for (int i = 0; i < contentBox.transform.childCount; i++)
        {
            contentBox.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < Moduless.leaderCampain.Count; i++)
        {
            GameObject card = contentBox.transform.GetChild(i).gameObject;
            card.SetActive(true);
            card.name = Moduless.leaderCampain[i].namePlayer;
            card.transform.FindChild("TextStt").GetComponent<Text>().text = i + 1 + "";
            card.transform.FindChild("TextName").GetComponent<Text>().text = Moduless.leaderCampain[i].namePlayer + "";
            card.transform.FindChild("TextScore").GetComponent<Text>().text = Moduless.leaderCampain[i].score + "";
            card.transform.FindChild("TextLevel").GetComponent<Text>().text = Moduless.leaderCampain[i].level + "";
            card.transform.FindChild("imgavatar").GetComponent<Image>().sprite = Moduless.GetAvatar(
            FindObjectOfType<LeaderBoardController>().spriteAvatar, Moduless.leaderCampain[i].avatar);
        }

        //
    }
}
