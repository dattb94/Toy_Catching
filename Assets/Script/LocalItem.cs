using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalItem : MonoBehaviour {
    public int index;
    void OnMouseOver()
    {
        Modules.localMouse = index;
    }
}
