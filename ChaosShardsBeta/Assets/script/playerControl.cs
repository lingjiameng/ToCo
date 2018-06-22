using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/***************/
//cardControl 及 player 类
//待完善
/****************/

public class playerControl : MonoBehaviour
{
    /*****player info********/
    public int playerHealth;//玩家血量
    public int distance;//公共数据距离
    /*************/

    /*******game info**********/
    public enum GameStageSate
    {
        begin,
        playerGetCard,
        playerPrepare,
        playerAction,
        playerEnd,
        EnemyPrepare,
        EnemyAction,
        EnemyEnd,
        Wait,
        DisCardWait,
        AllWait
    }

    /******************/

    /************/
    //牌的图片路径  
    private string fullPath = "Assets/Resources/image/cards/deck/";

    /**stage state**/
    public int cardManagerState = 1;//游戏进行阶段标记

    public GameStageSate myState;


    /********card list**********/
    public List<string> CardNames;  //所有牌集合
    public List<string> CardHeapList;//牌堆名字队列;

    public List<string> CardList = new List<string>();//卡牌名字队列
    public string ToUseCard;//要使用的卡牌名字
    public List<string> UsedCardList = new List<string>();//使用过卡牌名字队列

    /*******card number and text*********/
    private Text cardHeapCountText;//数量维护标记
    private Text gameStageStateText; //游戏阶段标记
    private Text distanceText; //距离标记维护
    private Text roundCountText; //回合数文本标记

    public int CardHeapNum = 12;//牌堆默认总牌数

    /******prefab or resources*****/
    public GameObject prefab;   //预制件
    public GameObject coverPrefab;      //背面排预制件

    /***postion control***/
    private Transform originPos1; //牌的初始位置1
    //private Transform originPos2; //牌的初始位置2 not nessery
    public Transform heapPos;           //牌堆位置
    public Transform playerHeapPos;    //玩家牌堆位置

    public float offsetX = 60;


    /*******object and its list*******/
    GameObject playerSelf;//玩家对象
    GameObject btnPanel;// 按键组合对象
    GameObject ensureBtn;// 确认键
    GameObject gameFunc;//逻辑处理对象

    private List<GameObject> cards = new List<GameObject>();//卡牌对象列表
    private List<GameObject> covers = new List<GameObject>();   //背面卡牌对象，发牌结束后销毁


    /*****var and parameter*******/
    private int round=0;//回合数标记
    public float dealCardSpeed = 20;  //发牌速度
    //int count = 0;
    //public float cardSize = 1f;//卡片缩放
    public float posOffset = 2.2f; //卡片偏移



    void Awake()
    {
        CardNames = GetCardNames();
        CardHeapList = CardNames;
        //Debug.Log(cardNames.Join(","));

        myState= GameStageSate.begin;//置为开始阶段；
    }
    /*
    GameObject card01;
    //public Transform card01;//意图表示第一张牌的位置  

    //public Transform card02;

    public GameObject cardsprefab;

    public float thedistance=45;//两张牌的距离  
    */

    // Use this for initialization
    void Start()
    {
        /******预制件加载***********/
        prefab = (GameObject)Resources.Load("prefab/cardPrefab");
        coverPrefab = (GameObject)Resources.Load("prefab/coverPrefab");

        /********对象加载******/
        playerSelf = GameObject.Find("self");
        btnPanel = GameObject.Find("BtnPanel");
        ensureBtn = GameObject.Find("UseCardBtn");
        gameFunc = GameObject.Find("GameManager");

        /************位置加载**************/
        originPos1 = GameObject.Find("CardPanel").GetComponent<RectTransform>();
        heapPos = GameObject.Find("cardHeap").GetComponent<RectTransform>();
        playerHeapPos = GameObject.Find("CardPanel").GetComponent<RectTransform>();

        /*********文本组件加载*********/
        cardHeapCountText = GameObject.Find("cardHeapText").GetComponent<Text>();
        gameStageStateText = GameObject.Find("GameStageText").GetComponent<Text>();
        distanceText= GameObject.Find("DistanceText").GetComponent<Text>();
        roundCountText = GameObject.Find("RoundCountText").GetComponent<Text>();
        /*
        GameObject card = (GameObject)Resources.Load("prefab/cardPre");
        card01 = Instantiate(card);

        Debug.Log("create sucess!");
        GameObject mUICanvas = GameObject.Find("CardPanel");
        card01.transform.parent = mUICanvas.transform;
        CardList.Add(card01);
        */

        //OnTestClick();

        //隐藏不必要按键
        ButtonDisable();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(Getcard(2));
        }
        */

        /****文本标记维护****/
        //cardHeapCountText = GameObject.Find("cardHeap").GetComponent<Text>();
        if (!!cardHeapCountText)
        {
            cardHeapCountText.text = CardHeapList.Count.ToString();
        }

        if (!!cardHeapCountText)
        {
            distanceText.text ="距离:"+ playerSelf.GetComponent<effect_self>().Distance().ToString();
        }

        /***弃牌阶段侦听***/
        if (myState == GameStageSate.DisCardWait && CardList.Count <= 2)
        {
            ButtonDisable();
            myState = GameStageSate.EnemyPrepare;
            //break;
        }

        App();

    }
    public void App()
    {
        /****游戏阶段逻辑顺序****/
        /************* 游戏准备阶段*****************/
        if (myState == GameStageSate.begin)
        {
            myState = GameStageSate.AllWait;
            //UI刷新待加入
            if (!!gameStageStateText)//游戏阶段文本刷新
            {
                gameStageStateText.text = "游戏开始";
            }


            OnBegin();//牌的处理函数
            //myState = GameStageSate.playerGetCard; 在DealCard 函数结尾
        }

        /****************己方回合****************/

        else if(myState ==GameStageSate.playerGetCard)
        {
            myState = GameStageSate.AllWait;

            //UI刷新待加入
            round += 1;
            roundCountText.text = "第" + round.ToString() + "回合";//回合数文本刷新

            if (!!gameStageStateText)//游戏阶段文本刷新
            {
                gameStageStateText.text = "摸牌阶段";
            }
            //行动力刷新
            playerSelf.GetComponent<effect_self>().IncreaseMoveEnergy(1);

            //洗牌
            if (CardHeapList.Count < 2)
            {
                ShuffleCards();
            }
            StartCoroutine(Getcard(2));//摸牌
            //myState = GameStageSate.playerPrepare; 在GetCard 函数结尾
        }
        else if (myState == GameStageSate.playerPrepare)
        {
            myState = GameStageSate.AllWait;
            //UI刷新待加入
            if (!!gameStageStateText)//游戏阶段文本刷新
            {
                gameStageStateText.text = "准备阶段";
            }
            //TODO:状态调整
            myState = GameStageSate.playerAction; //待加入到函数结尾
        }
        else if (myState == GameStageSate.playerAction)
        {
            myState = GameStageSate.Wait;
            //UI刷新待加入
            if (!!gameStageStateText)//游戏阶段文本刷新
            {
                gameStageStateText.text = "行动阶段";
            }


            ButtonEnable();//显示按钮
                           //进行众多卡牌操作


            //TODO:增加结束按钮来进入下一阶段
            //myState = GameStageSate.playerEnd  加入到结束按钮中  finish  *****not work*****
            
        }
        else if (myState == GameStageSate.playerEnd)
        {
            myState = GameStageSate.DisCardWait;
            //UI刷新待加入
            if (!!gameStageStateText)//游戏阶段文本刷新
            {
                gameStageStateText.text = "结束阶段";
            }

            ButtonForDiscard();
            //弃牌 待加入  即使用牌直到牌等于两张
            //TODO:写一个弃牌函数  弃牌函数结尾进入下一阶段敌人准备阶段  

            //ButtonDisable();
            //myState = GameStageSate.EnemyPrepare; //待加入到函数结尾
        }

        /*****************敌人回合*********************/
        else if(myState == GameStageSate.EnemyPrepare)
        {
            myState = GameStageSate.AllWait;
            //UI刷新待加入
            if (!!gameStageStateText)//游戏阶段文本刷新
            {
                gameStageStateText.text = "敌方回合";
            }

            myState = GameStageSate.EnemyAction; //待加入到函数结尾
        }
        else if(myState == GameStageSate.EnemyAction)
        {
            myState = GameStageSate.AllWait;

            /**************TODO:增加敌人动作演示*****************/
            playerSelf.GetComponent<effect_self>().TakeDamage(1);
            playerSelf.GetComponent<effect_self>().AddselfBlood(-1);

            /*****************************/
            myState = GameStageSate.EnemyEnd; //待加入到函数结尾
        }
        else if(myState == GameStageSate.EnemyEnd)
        {
            myState = GameStageSate.AllWait;

            myState = GameStageSate.playerGetCard;//待加入到函数结尾
        }
       
    }

    //开始阶段 牌的处理
    public void OnBegin()
    {
        //弃牌堆 和牌堆转化 待调整

        ClearCards(); //清空牌局
        ShuffleCards(); //牌堆洗牌
        StartCoroutine(DealCards(3)); //初始发牌
    }

    //使button生效时
    public void ButtonEnable()
    {
        btnPanel.SetActive(true); //激活btnPanel
        ensureBtn.SetActive(true); //激活确认键
    }

    //调整Button用于弃牌
    public void ButtonForDiscard()
    {
        btnPanel.SetActive(false); //隐藏btnPanel
        ensureBtn.SetActive(true); //激活确认键
    }

    //使Button 全部失效
    public void ButtonDisable()
    {
        btnPanel.SetActive(false); //隐藏btnPanel
        ensureBtn.SetActive(false); //隐藏确认键
    }

    //增加一张卡牌
    public void AddCard(string cardName)
    {
        //将牌加入手牌列表
        CardList.Add(cardName);

        //维护 牌堆卡牌 和 手牌 数量标记
        //cardCountText.text = (CardHeapNum - CardList.Count).ToString();
    }

    //清空所有卡牌 信息
    public void DropCards()
    {
        CardList.Clear();
    }

    //清空所有卡牌 对象
    public void DestroyAllCards()
    {
        cards.ForEach(Destroy);
        cards.Clear();
    }

    //整理手牌  //动画开关 可选
    public void GenerateAllCards(bool AnimationSwitch)
    {
        //计算每张牌的偏移
        //var offsetX = originPos2.position.x - originPos1.position.x;
        //获取最左边的起点
        //int leftCount = (cardInfos.Count / 2);
        var startPos = playerHeapPos.position + Vector3.right * 10f;
        float speed = 0f;
        for (int i = 0; i < CardList.Count; i++)
        {
            if (AnimationSwitch)
            {
                speed = 2f;
                //生成卡牌
                var card = Instantiate(prefab, startPos, Quaternion.identity, playerHeapPos);// TODO: need to fix
                card.GetComponent<RectTransform>().position = startPos;//暂时的处理方法
                card.GetComponent<RectTransform>().localScale = Vector3.one * 1f;
                card.GetComponent<card>().InitImage(CardList[i]);

                offsetX = card.GetComponent<RectTransform>().rect.width * posOffset;
                var targetPos = startPos + Vector3.right * offsetX * i;
                card.transform.SetAsLastSibling();
                //动画移动
                iTween.MoveTo(card, targetPos, speed);

                cards.Add(card);
            }
            else
            {
                //生成卡牌
                var card = Instantiate(prefab, startPos, Quaternion.identity, playerHeapPos);// TODO: need to fix

                card.GetComponent<RectTransform>().localScale = Vector3.one * 1f;
                card.GetComponent<card>().InitImage(CardList[i]);

                offsetX = card.GetComponent<RectTransform>().rect.width * posOffset;
                var targetPos = startPos + Vector3.right * offsetX * i;
                card.GetComponent<RectTransform>().position = targetPos;//暂时的处理方法
                card.transform.SetAsLastSibling();
                //动画移动
                //iTween.MoveTo(card, targetPos, speed);

                cards.Add(card);

            }


        }
    }

    /*
    /// <summary>
    /// 点击卡牌处理
    /// </summary>
    /// <param name="data"></param>
    public void CardClick(BaseEventData data)
    {
        //叫牌或出牌阶段才可以选牌
        if (cardManagerState == 3 ||
            false)
        {
            var eventData = data as PointerEventData;
            if (eventData == null) return;

            var card = eventData.pointerCurrentRaycast.gameObject.GetComponent<card>();
            if (card == null) return;

            card.SetSelectState();
        }
    }
    */

    //使用卡牌
    public void UseCard()
    {
        //第一步检查
        //再生效并触发UI逻辑
        //结束
        for (int i = cards.Count - 1; i >= 0; i--)
        {
            if (cards[i].GetComponent<card>().SelectState())
            {
                UsedCardList.Add(CardList[i]);
                ToUseCard = CardList[i];
                Debug.Log(CardList[i]);
                CardList.RemoveAt(i);
            }
        }
        DestroyAllCards();
        GenerateAllCards(false);

        //Debug.Log(ToUseCardList[0]);
        //effect(ToUseCardList[0]);


        //
        gameFunc.GetComponent<gameManager>().one_card(ToUseCard);


        //ToUseCardList.Clear();
    }

    //弃牌函数
    public void DisCard()
    {
        //第一步检查
        
       
        //再生效并触发UI逻辑
        //结束
        for (int i = cards.Count - 1; i >= 0; i--)
        {
            if (cards[i].GetComponent<card>().SelectState())
            {
                UsedCardList.Add(CardList[i]);
                ToUseCard = CardList[i];
                Debug.Log(CardList[i]);
                CardList.RemoveAt(i);
            }
        }
        DestroyAllCards();
        GenerateAllCards(false);

        //Debug.Log(ToUseCardList[0]);
        //effect(ToUseCardList[0]);

        //ToUseCardList.Clear();
        //结束处理

        return;
    }


    //获得卡牌即摸牌  
    public IEnumerator Getcard(int Num)
    {
        //进入发牌阶段
        //cardManagerState = CardManagerStates.DealCards;

        //显示牌堆
        //heapPos.gameObject.SetActive(true);

        //从牌堆中取出制定数量的牌，
        //维护牌堆列表
        //播放相应动画
        DestroyAllCards();

        CardHeapNum = CardHeapList.Count();
        for (int i = CardHeapNum - 1; i >= CardHeapNum - Num; i--)
        {
            string cardName = CardHeapList[i];

            //给当前玩家发一张牌
            //playerSelf.GetComponent<player>().AddCard(cardName);
            AddCard(cardName);
            CardHeapList.RemoveAt(i);


            var cover = Instantiate(coverPrefab, heapPos.position, Quaternion.identity, heapPos.parent.transform);
            //cover.GetComponent<RectTransform>().position = heapPos.position;//暂时的处理方法
            cover.GetComponent<RectTransform>().localScale = Vector3.one;
            covers.Add(cover);
            iTween.MoveTo(cover, playerHeapPos.position, 0.3f);

            yield return new WaitForSeconds(1 / dealCardSpeed);
        }

        //隐藏牌堆
        //heapPos.gameObject.SetActive(false);
        //playerHeapPos.gameObject.SetActive(false);

        //动画结束，进入叫牌阶段
        yield return new WaitForSeconds(1f);
        covers.ForEach(Destroy);
        covers.Clear();


        //显示玩家手牌
        if (playerSelf != null)
        {
            //playerSelf.GetComponent<player>().GenerateAllCards();
            GenerateAllCards(true);
        }
        //cardManagerState = CardManagerStates.Bid;

        //摸牌后进入准备阶段
        myState = GameStageSate.playerPrepare;


    }


    private List<string> GetCardNames()
    {
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*.png", SearchOption.AllDirectories);

            return files.Select(s => Path.GetFileNameWithoutExtension(s.Name)).ToList();
        }
        return null;
    }

    public void ShuffleCards()
    {
        //进入洗牌阶段
        //cardManagerState = CardManagerStates.ShuffleCards;
        if(CardHeapList.Count<2)
        {
            CardHeapList.InsertRange(0, UsedCardList);
            UsedCardList.Clear();
        }
        CardHeapList = CardHeapList.OrderBy(c => Guid.NewGuid()).ToList();
    }


    /// <summary>
    /// 发牌
    /// </summary>
    public IEnumerator DealCards(int Num)
    {
        //进入发牌阶段
        //cardManagerState = CardManagerStates.DealCards;

        //显示牌堆
        //heapPos.gameObject.SetActive(true);

        //从牌堆中取出制定数量的牌，
        //维护牌堆列表
        //播放相应动画
        CardHeapNum = CardHeapList.Count();
        for (int i = CardHeapNum - 1; i >= CardHeapNum - Num; i--)
        {
            string cardName = CardHeapList[i];

            //给当前玩家发一张牌
            //playerSelf.GetComponent<player>().AddCard(cardName);
            AddCard(cardName);
            CardHeapList.RemoveAt(i);


            var cover = Instantiate(coverPrefab, heapPos.position, Quaternion.identity, heapPos.parent.transform);
            //cover.GetComponent<RectTransform>().position = heapPos.position;//暂时的处理方法
            cover.GetComponent<RectTransform>().localScale = Vector3.one;
            covers.Add(cover);
            iTween.MoveTo(cover, playerHeapPos.position, 0.3f);

            yield return new WaitForSeconds(1 / dealCardSpeed);
        }

        //隐藏牌堆
        //heapPos.gameObject.SetActive(false);
        //playerHeapPos.gameObject.SetActive(false);

        //动画结束，进入叫牌阶段
        yield return new WaitForSeconds(1f);
        covers.ForEach(Destroy);
        covers.Clear();


        //显示玩家手牌
        if (playerSelf != null)
        {
            //playerSelf.GetComponent<player>().GenerateAllCards();
            GenerateAllCards(true);
        }
        //cardManagerState = CardManagerStates.Bid;

        //第一次发牌后进入摸牌阶段
        myState = GameStageSate.playerGetCard;
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
            DestroyAllCards();
        };
    }

    


}

