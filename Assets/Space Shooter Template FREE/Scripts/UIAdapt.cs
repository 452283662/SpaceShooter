using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdapt : MonoBehaviour {

    private void Start(){
        float canvasWidth = GameObject.Find("Canvas").GetComponent<RectTransform>().rect.width;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(7f,0f,0f));
        Camera uicamera = GameObject.Find("UICamera").GetComponent<Camera>();
        Vector3 worldPos = uicamera.ScreenToWorldPoint(screenPos);
        Vector3 localPos = transform.InverseTransformPoint(worldPos);
        if (canvasWidth > localPos.x * 2){
            float offest = canvasWidth / 2f - localPos.x;
            RectTransform rtTrans = GetComponent<RectTransform>();
            rtTrans.offsetMin = new Vector2(offest, 0f);
            rtTrans.offsetMax = new Vector2(-offest, 0f);
        }
    }
}
