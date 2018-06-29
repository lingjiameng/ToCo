using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveEnergyControl : MonoBehaviour {
    //string MoveEnergyNum;

    List<GameObject> Energys=new List<GameObject>();
    // Use this for initialization
    void Start () {
        //MoveEnergyNum = gameObject.GetComponent<Text>().text;
        Energys.Add(GameObject.Find("energy1"));
        Energys.Add(GameObject.Find("energy2"));
        Energys.Add(GameObject.Find("energy3"));
    }
	
	// Update is called once per frame
	void Update () {
        int i;
        if(Energys!=null)
        {
            for (i = 0; i < GameObject.Find("self").GetComponent<effect_self>().moveEnergyValue; i++)
            {
                if (Energys[i] != null) Energys[i].SetActive(true);
            }
            for (; i < Energys.Count; i++)
            {
                if (Energys[i] != null) Energys[i].SetActive(false);
            }
        }
        
	}
}
