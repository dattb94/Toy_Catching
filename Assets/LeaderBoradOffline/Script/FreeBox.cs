using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FreeBox : MonoBehaviour {
	void Start () {
    }
    void Update()
    {
        SetDataContent();
    }
    //xu ly phan du content
    public Image imgCard;
    public GameObject contentBox, cardPlayer;
    void SetDataContent()
    {
        Modules.LoadLeaderBoardFree();
        print(Modules.leaderFree.Count);
        //set height content
        float space = contentBox.GetComponent<VerticalLayoutGroup>().spacing;
        float cardHeigh = imgCard.GetComponent<RectTransform>().sizeDelta.y;
        int count = Modules.leaderFree.Count;
        contentBox.GetComponent<RectTransform>().sizeDelta= new Vector2(contentBox.GetComponent<RectTransform>().sizeDelta.x, 
            count*(cardHeigh+space));
        //
        //Set data cho cac the? player 
        for (int i = 0; i < contentBox.transform.childCount; i++)
        {
            contentBox.transform.GetChild(i).gameObject.SetActive(false);
        }
        if (Modules.leaderFree.Count > 0)
        {
            for (int i = 0; i < Modules.leaderFree.Count; i++)
            {
                GameObject card = contentBox.transform.GetChild(i).gameObject;
                card.SetActive(true);
                card.name = Modules.leaderFree[i].namePlayer;
                card.transform.FindChild("TextStt").GetComponent<Text>().text = i + 1 + "";
                card.transform.FindChild("TextName").GetComponent<Text>().text = Modules.leaderFree[i].namePlayer + "";
                card.transform.FindChild("TextScore").GetComponent<Text>().text = Modules.leaderFree[i].score + "";
                card.transform.FindChild("imgavatar").GetComponent<Image>().sprite = Modules.GetAvatar(Modules.leaderFree[i].avatar);
            }
        }
        //
    }
    //
}