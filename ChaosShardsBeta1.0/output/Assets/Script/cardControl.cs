using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/***************/
//cardControl 及 player 类
//待完善
/****************/

public class cardControl : MonoBehaviour {
    public float dealCardSpeed = 20;  //发牌速度
    GameObject playerSelf;//玩家对象
    

    public GameObject coverPrefab;      //背面排预制件
    public Transform heapPos;           //牌堆位置

    public Transform playerHeapPos;    //玩家牌堆位置
    public string[] cardNames;  //所有牌集合
    private List<GameObject> covers = new List<GameObject>();   //背面卡牌对象，发牌结束后销毁

    int count=0;

    void Awake()
    {
        cardNames = GetCardNames();
        
        //playerSelf.GetComponent<player>().test();

        //GameObject.Find("player").GetComponent<>
    }

    // Use this for initialization
    void Start()
    {
        playerSelf = GameObject.Find("self");
        coverPrefab = (GameObject)Resources.Load("prefab/coverPrefab");
        heapPos = GameObject.Find("cardHeap").GetComponent<RectTransform>();
        playerHeapPos = GameObject.Find("CardPanel").GetComponent<RectTransform>();
        OnTestClick();
        /*
        GameObject card = (GameObject)Resources.Load("prefab/cardPre");
        card01 = Instantiate(card);

        Debug.Log("create sucess!");
        GameObject mUICanvas = GameObject.Find("CardPanel");
        card01.transform.parent = mUICanvas.transform;
        CardList.Add(card01);
        */


    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.S))
        {
            Getcard();
        }
        */
        

    }

    public void ShuffleCards()
    {
        //进入洗牌阶段
        //cardManagerState = CardManagerStates.ShuffleCards;
        //cardNames = cardNames.OrderBy(c => Guid.NewGuid()).ToArray();
    }
    /// <summary>
    /// 发牌
    /// </summary>
    public IEnumerator DealCards()
    {
        //进入发牌阶段
        //cardManagerState = CardManagerStates.DealCards;

        //显示牌堆
        //heapPos.gameObject.SetActive(true);
        

        foreach (var cardName in cardNames)
        {
            //给当前玩家发一张牌
            //playerSelf.GetComponent<player>().AddCard(cardName);


            var cover = Instantiate(coverPrefab, heapPos.position, Quaternion.identity, heapPos.transform);
            cover.GetComponent<RectTransform>().localScale = Vector3.one;
            covers.Add(cover);
            iTween.MoveTo(cover, playerHeapPos.position, 0.3f);
            count++;
            yield return new WaitForSeconds(1 / dealCardSpeed);
            if (count >= 5) break;

        }

        //隐藏牌堆
        //heapPos.gameObject.SetActive(false);
        //playerHeapPos.gameObject.SetActive(false);

        //动画结束，进入叫牌阶段
        yield return new WaitForSeconds(2f);
        covers.ForEach(Destroy);
        covers.Clear();


        //显示玩家手牌
        if (playerSelf != null)
        {
            //playerSelf.GetComponent<player>().GenerateAllCards();
        }


        
        //cardManagerState = CardManagerStates.Bid;
    }

    /// <summary>
    /// 清空牌局
    /// </summary>
    public void ClearCards()
    {
        //清空所有玩家卡牌
        if (playerSelf != null)
        {
            //playerSelf.GetComponent<player>().DestroyAllCards();
        };
    }


   

    private string[] GetCardNames()
    {
        //路径  
        string fullPath = "Assets/Resources/image/cards/pkp/";

        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*.jpg", SearchOption.AllDirectories);

            return files.Select(s => Path.GetFileNameWithoutExtension(s.Name)).ToArray();
        }
        return null;
    }

    //for test
    public void OnTestClick()
    {
        ClearCards();
        ShuffleCards();
        StartCoroutine(DealCards());
    }


    //增加一张


    /*
    //获得卡牌即摸牌  
    public void Getcard()
    {
        GameObject card = (GameObject)Resources.Load("prefab/cardPre");
        CardList.Add(Instantiate(card));
        int i = CardList.Count-1;
        Vector3 toposition = card01.transform.position + new Vector3(thedistance, 0, 0) * i;//获得卡牌到达的位置，（现有牌数量的最后面，即与第一张牌的距离位置）  

        CardList[i].transform.position = toposition;

    }
    */
}
