using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distance : MonoBehaviour {

    public List<GameObject> tiles = new List<GameObject>();
    public GameObject self;
    public GameObject enemy;
    private GameObject Dis;
    private new Transform transform;
    private GameObject tmp;

    public GameObject prefab_ParticalSys;
    
    private float DISPLAY_FALSE_TIME;
    private float DISPLAY_TRUE_TIME;
    /* 
     * 关于距离的说明
     * 初始 1   11
     * 最小0  最大12
    */
    public int position_enemy;
    public int position_self;

    // Use this for initialization
    void Start () {

        // 定义一些变量
        DISPLAY_FALSE_TIME = 1.10f;
        DISPLAY_TRUE_TIME = 1f;

        // 获取游戏

        Dis = GameObject.Find("DistanceTiles");
        self = GameObject.Find("self");
        enemy = GameObject.Find("Enemy");
        transform = Dis.GetComponent<Transform>();


        Debug.Log("开始加载地砖对象");
        foreach (Transform child in transform)
        {
            tiles.Add(child.gameObject);
            //Debug.Log(child.gameObject.name);
        }
        Debug.Log("地砖对象加载完毕");

        // 初始化地砖
        tiles[0].SetActive(false);
        tiles[12].SetActive(false);


        //Instantiate(prefab_ParticalSys, tiles[6].GetComponent<Transform>().position, tiles[6].GetComponent<Transform>().rotation);
    }
	
	// Update is called once per frame
	void Update () {
        //position_enemy = GameObject.Find("Enemy").GetComponent<effect_enemy>().position;
        //position_self = GameObject.Find("self").GetComponent<effect_self>().position;
    }

    //英雄移动
    /* 
     * move 0 不移动
     * + 向前走
     * - 向后走
     */
    public void Move_Self(int move)
    {
        position_enemy = GameObject.Find("Enemy").GetComponent<effect_enemy>().position;
        position_self = GameObject.Find("self").GetComponent<effect_self>().position;

        if (move == 0)
            return;
        if(move > 0)
        {
            //  向前走
            iTween.MoveTo(self, iTween.Hash("position", tiles[position_self].GetComponent<Transform>().position, "time", 1f, "easetype", iTween.EaseType.easeInOutBack));
            //self.GetComponent<Transform>().position = tiles[position_self].GetComponent<Transform>().position;
            for (int i = 1; i <= move; i++)
            {
                Display_False_tile(position_self - i);
            }
        }
        else
        {
            // 向后走
            for (int p = position_self - move - 1; p >= position_self; p--)
            {
                Display_True_tile(p);
            }
            iTween.MoveTo(self, iTween.Hash("position", tiles[position_self].GetComponent<Transform>().position, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
            //self.GetComponent<Transform>().position = tiles[position_self].GetComponent<Transform>().position;
        }
    }


    // 敌人移动
    public void Move_Enemy(int move)
    {
        position_enemy = GameObject.Find("Enemy").GetComponent<effect_enemy>().position;
        position_self = GameObject.Find("self").GetComponent<effect_self>().position;

        if (move == 0)
            return;
        if (move > 0)
        {
            //  向前走
            iTween.MoveTo(enemy, iTween.Hash("position", tiles[position_enemy].GetComponent<Transform>().position, "time", 1f, "easetype", iTween.EaseType.easeInOutBack));
            //enemy.GetComponent<Transform>().position = tiles[position_enemy].GetComponent<Transform>().position;
            for (int p = position_enemy + move; p > position_enemy; p--)
            {
                Display_False_tile(p);
            }
            Debug.Log("A");
            Debug.Log(GameObject.Find("Enemy").GetComponent<effect_enemy>().position);
        }
        else
        {
            // 向后走
            for (int p = position_enemy + move + 1; p <= position_enemy; p++)
            {
                Display_True_tile(p);
            }

            //Debug.Log("Attention!!!");
            //Debug.Log(position_enemy);
            //iTween.MoveTo(self, iTween.Hash("position", tiles[position_self].GetComponent<Transform>().position, "time", 1f, "easetype", iTween.EaseType.easeInOutBack));
            iTween.MoveTo(enemy, iTween.Hash("position", tiles[position_enemy].GetComponent<Transform>().position, "time", 1f, "easetype", iTween.EaseType.easeOutQuad));
            //enemy.GetComponent<Transform>().position = tiles[position_enemy].GetComponent<Transform>().position;
            //Debug.Log(tiles[12].GetComponent<Transform>().position.ToString() + enemy.GetComponent<Transform>().position.ToString());
            //self.GetComponent<Transform>().position = tiles[position_self].GetComponent<Transform>().position;
        }
    }

    private void Display_True_tile(int number)
    {
        //tmp = Instantiate(prefab_ParticalSys, tiles[number].GetComponent<Transform>().position, tiles[number].GetComponent<Transform>().rotation);
        //Invoke("Destory", DISPLAY_TRUE_TIME);
        tiles[number].SetActive(true);
    }

    public void Destory()
    {
        DestroyObject(tmp);
    }

    private void Display_False_tile(int number)
    {
        //tmp = Instantiate(prefab_ParticalSys, tiles[number].GetComponent<Transform>().position, tiles[number].GetComponent<Transform>().rotation);
        //Invoke("Destory", DISPLAY_FALSE_TIME);
        tiles[number].SetActive(false);
    }
}
