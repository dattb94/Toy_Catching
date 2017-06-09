using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocateBar : MonoBehaviour {
    public LayerMask mark;
    public bool cham;
    void Update()
    {

        cham = Physics2D.OverlapCircle(transform.position, 0.05f, mark);
        if (cham)
        {
            GetComponent<Renderer>().enabled = false;
        }
        else GetComponent<Renderer>().enabled = true;
    }
}
