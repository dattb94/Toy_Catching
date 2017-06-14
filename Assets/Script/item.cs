using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour {
    public int type;
    public bool hasTop;
    public bool isIron;
    public bool isBoom;
    public GameObject itemTop;
    float start_x;
    public bool beChoise = false;
    float dem = 0;
    void Start() {
        
        start_x = transform.position.x;
        if(transform.name!= "item5(Clone)")
            transform.FindChild("image").GetComponent<Animator>().SetBool("bang", false);
    }
    void Update()
    {
        transform.position = new Vector3(start_x, transform.position.y, 0);
        if (Input.GetKeyDown(KeyCode.D))
        {
            Destroy(gameObject);
        }
        if (Modules.isSpawning)
        {
            transform.position -= new Vector3(0, Modules.DistanceItems(), 0);
        }
        if (Modules.countBeChoise >= 4 && beChoise)
        {
            _DesTroy();
        }
        if (Modules.keepItem)
            beChoise = false;
        if (beChoise)
        {
            dem += Time.deltaTime;
            if (dem > 0.5f)
            {
                beChoise = false;
                dem = 0;
            }
        }
    }
    public void _DesTroy()
    {
        if (transform.name != "item5")
        {
            if(!GameObject.Find("audestroyItem"))
                Modules.PlayAudio("destroyItem",0.5f);
            transform.FindChild("image").GetComponent<Animator>().SetBool("bang", true);
        }
        Destroy(gameObject, 0.2f);
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "checkLost")
        {
            if (!GameObject.Find("CampaignScene"))
            {
                GameManager.beLost = true;
            }
            if (GameObject.Find("CampaignScene"))
            {
                CampaignSceneManager.beLost = true;
                print("lost");
            }
        }
        if (col.gameObject.tag == "item")
        {
            if (col.gameObject.GetComponent<item>().type == type&&col.gameObject.GetComponent<item>().beChoise)
            {
                if (!beChoise)
                    beChoise = true;
            }
        }
    }
}
