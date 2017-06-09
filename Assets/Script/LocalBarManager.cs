using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalBarManager : MonoBehaviour {
    public static GameObject A; 
    // Use this for initialization
    void Start() {
        transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, GameObject.Find("p1").transform.position.y, transform.GetChild(0).transform.position.z);
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.position = new Vector3(
                transform.GetChild(0).transform.position.x,
                transform.GetChild(0).transform.position.y-Modules.DistanceItems()*i,
                transform.GetChild(0).transform.position.z);
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).GetComponent<LocateBar>().cham)
            {
                A = transform.GetChild(i).gameObject;
                break;
            }
        }
        if (!Modules.keepItem)
            GameObject.Find("rootLocalBar").transform.position = transform.position;
        else
        {
            GameObject.Find("rootLocalBar").transform.position = new Vector3(transform.position.x-Modules.DistanceItems()/2, transform.position.y - Modules.DistanceItems()/2 , 1);
        }
	}
}
