using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CampainBox : MonoBehaviour {

    //void Start()
    //{
    //    SetDataContent();
    //}
    ////xu ly phan du content
    //public Image imgCard;
    //public GameObject contentBox, cardPlayer;
    //void SetDataContent()
    //{
    //    Modules.LoadLeaderBoardCampain();
    //    //set height content
    //    float space = contentBox.GetComponent<VerticalLayoutGroup>().spacing;
    //    float cardHeigh = imgCard.GetComponent<RectTransform>().sizeDelta.y;
    //    int count = Modules.leaderCampain.Count;
    //    contentBox.GetComponent<RectTransform>().sizeDelta = new Vector2(contentBox.GetComponent<RectTransform>().sizeDelta.x,
    //        count * (cardHeigh + space));
    //    //
    //    //Set data cho cac the? player 
    //    for (int i = 0; i < contentBox.transform.childCount; i++)
    //    {
    //        contentBox.transform.GetChild(i).gameObject.SetActive(false);
    //    }
    //    for (int i = 0; i < Modules.leaderCampain.Count; i++)
    //    {
    //        GameObject card = contentBox.transform.GetChild(i).gameObject;
    //        card.SetActive(true);
    //        card.name = Modules.leaderCampain[i].namePlayer;
    //        card.transform.FindChild("TextStt").GetComponent<Text>().text = i + 1 + "";
    //        card.transform.FindChild("TextName").GetComponent<Text>().text = Modules.leaderCampain[i].namePlayer + "";
    //        card.transform.FindChild("TextScore").GetComponent<Text>().text = Modules.leaderCampain[i].score + "";
    //        card.transform.FindChild("TextLevel").GetComponent<Text>().text = Modules.leaderCampain[i].level + "";
    //        card.transform.FindChild("imgavatar").GetComponent<Image>().sprite = Modules.GetAvatar(Modules.leaderCampain[i].avatar);
    //    }

    //    //
    //}
}
