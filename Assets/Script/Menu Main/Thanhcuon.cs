using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Thanhcuon : MonoBehaviour
{
    public Scrollbar scb;
    public RectTransform min, max;
    void Update()
    {
        GetComponent<RectTransform>().position = new Vector3(min.position.x+ ((max.position.x - min.position.x) *scb.value), 
            min.position.y, min.position.z);
        AudioListener.volume = scb.value;
    } 
}
