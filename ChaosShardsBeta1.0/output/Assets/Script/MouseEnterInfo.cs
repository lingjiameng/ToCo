using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseEnterInfo : MonoBehaviour {

    public GameObject scriptObject;

    private bool IsShowTip;

    private string CardInfo;
    private string CardName;
	// Use this for initialization
	void Start () {
        scriptObject = GameObject.Find("check");
        IsShowTip = false;
        CardName = this.name;
    }
	
	// Update is called once per frame
	void Update () {
        //if (IsShowTip)
        //{
        //    // 显示卡牌信息
        //    Debug.Log("显示卡牌信息");
        //    Debug.Log(CardName);
        //    // GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 40), "afdasdfasdf");
        //}
    }

    void OnMouseEnter()
    {
        IsShowTip = true;
        Debug.Log (CardName);//可以得到物体的名字

    }
    void OnMouseExit()
    {
        IsShowTip = false;
    }

    void OnGUI()
    {
        if (IsShowTip)
        {
            // 显示卡牌信息
            Debug.Log("显示卡牌信息");
            Debug.Log(CardName);
            GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 100, 40), "afdasdfasdf");
        }
    }

}
