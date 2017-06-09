using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTop : MonoBehaviour {
    public int type;
	// Use this for initialization
	void Start () {
        type = transform.parent.GetComponent<item>().type;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "item" )
        {
            if (col.GetComponent<item>())
            {
                if (col.GetComponent<item>().type == type)
                {
                    transform.parent.GetComponent<item>().hasTop = true;
                    transform.parent.GetComponent<item>().itemTop = col.gameObject;
                }
                else
                {
                    transform.parent.GetComponent<item>().hasTop = false;
                    transform.parent.GetComponent<item>().itemTop = null;
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "item")
        {
            transform.parent.GetComponent<item>().hasTop = false;
            transform.parent.GetComponent<item>().itemTop = null;
        }
    }
}
