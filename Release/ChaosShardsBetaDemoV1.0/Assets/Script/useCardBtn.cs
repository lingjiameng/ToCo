using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class useCardBtn : MonoBehaviour {
    private GameObject playerSelf;

    // Use this for initialization
    void Start () {
        playerSelf = GameObject.Find("self");
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        if(playerSelf.GetComponent<playerControl>().myState==playerControl.GameStageSate.Wait)
        {
            playerSelf.GetComponent<playerControl>().UseCard();
        }
        else if(playerSelf.GetComponent<playerControl>().myState == playerControl.GameStageSate.DisCardWait)
        {
            playerSelf.GetComponent<playerControl>().DisCard();
        }
        
    }
}
