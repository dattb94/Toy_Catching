using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FreeBox : MonoBehaviour {
	void Start () {
        SetDataContent();
    }
    //xu ly phan du content
    public Image imgCard;
    public GameObject contentBox, cardPlayer;
    void SetDataContent()
    {
        Moduless.LoadLeaderBoardFree();
        //set height content
        float space = contentBox.GetComponent<VerticalLayoutGroup>().spacing;
        float cardHeigh = imgCard.GetComponent<RectTransform>().sizeDelta.y;
        int count = Moduless.leaderFree.Count;
        contentBox.GetComponent<RectTransform>().sizeDelta= new Vector2(contentBox.GetComponent<RectTransform>().sizeDelta.x, 
            count*(cardHeigh+space));
        //
        //Set data cho cac the? player 
        for (int i = 0; i < contentBox.transform.childCount; i++)
        {
            contentBox.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < Moduless.leaderFree.Count; i++)
        {
            GameObject card = contentBox.transform.GetChild(i).gameObject;
            card.SetActive(true);
            card.name = Moduless.leaderFree[i].namePlayer;
            card.transform.FindChild("TextStt").GetComponent<Text>().text = i + 1 + "";
            card.transform.FindChild("TextName").GetComponent<Text>().text = Moduless.leaderFree[i].namePlayer + "";
            card.transform.FindChild("TextScore").GetComponent<Text>().text = Moduless.leaderFree[i].score + "";
            card.transform.FindChild("imgavatar").GetComponent<Image>().sprite = Moduless.GetAvatar(
            FindObjectOfType<LeaderBoardController>().spriteAvatar, Moduless.leaderFree[i].avatar);
        }

        //
    }
    //
}