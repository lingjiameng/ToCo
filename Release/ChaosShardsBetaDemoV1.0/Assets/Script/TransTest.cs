using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransTest : MonoBehaviour {

    public Transform cardTrans;
    public Transform cardPanelTrans;
    //public GameObject gameControl;  stage参数
    public int stage = 2;

    public int isSelected;
    

	// Use this for initialization
	void Start () {
        cardTrans = GetComponent<RectTransform>();
        cardPanelTrans = GameObject.Find("CardPanel").GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        if (stage == 2)
        {
            iTween.MoveTo(gameObject, cardPanelTrans.position, 2f);
            stage++;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            iTween.MoveTo(gameObject, cardTrans.position - Vector3.up * 10f, 1f);
        }
    }
}
