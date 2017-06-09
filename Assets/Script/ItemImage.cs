using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemImage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(GameObject.Find("ChoiseBar").transform.position.x, transform.position.y, transform.position.z);
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
	}
}
