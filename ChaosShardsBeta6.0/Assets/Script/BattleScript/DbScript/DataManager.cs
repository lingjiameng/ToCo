using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public class DataManager : MonoBehaviour
{
    #region class
    [System.Serializable]
    public class Card
    {
        public string name = null;
        public string ID = null;
        public string type = null;
        public string target = null;
        public string level = null;
        public int sheildbreak = 0;
        public int damage = 0;
        public int healthrecovery = 0;
        public int distance_low = 0;
        public int distance_high = 0;
        public int distance_change = 0;
        public string detail = null;
        public int energygain = 0;
        public int energycost = 0;
        public string intensify = null;
        public int state = 0;
        public string card_pic_name = null;
        public string card_pic_path = null;
        public int hit = 0;
    }
    /*[System.Serializable]
   class LevelList
    {
        public List<Level> levelList = new List<Level>();
    }*/
    #endregion

    #region variables
    [SerializeField]
    private TextAsset textJson_Ori;
    private TextAsset textJson_Int;
    [SerializeField]
    public List<Card> OriginalList = null;  //存有强化前的每张牌的信息
    public List<Card> IntensifiedList = null;//存有强化后的每张牌的信息
    public List<string> nameset; //存有所有卡牌的名称
    #endregion

    //形成两个存有卡牌信息的list
    public void CardSet()
    {
        textJson_Ori = Resources.Load("data/OriginalCards") as TextAsset;
        OriginalList = JsonMapper.ToObject<List<Card>>(textJson_Ori.text);
        textJson_Int = Resources.Load("data/IntensifiedCards") as TextAsset;
        IntensifiedList = JsonMapper.ToObject<List<Card>>(textJson_Int.text);
    }

    //传入一个OriginalList,形成卡牌的图片名称列表
    public List<string> NameSet()
    {
        List<string> nameset = new List<string>();
        for (int i = 0; i < OriginalList.Count; i++)
        {
            nameset.Add(OriginalList[i].card_pic_name);
        }
        return nameset;
    }

    //根据输入的图片名称,和OriginalList，返回一个图片地址
    public string Search(string card_pic_name)
    {
        for (int i = 0; i < OriginalList.Count; i++)
        {
            if (OriginalList[i].card_pic_name == card_pic_name)
            {
                return OriginalList[i].card_pic_path;
            }
        }
        for (int i = 0; i < IntensifiedList.Count; i++)
        {
            if (IntensifiedList[i].card_pic_name == card_pic_name)
            {
                return IntensifiedList[i].card_pic_path;
            }
        }
        return null;
    }


    //创建一个 游戏状态 类
    public class GameStatus
    {
        public float time = 0;
        public int round = 0;
        public int HP = 0;
        public int sheild = 0;
        public int state = 0;
        public int action = 0;
        public int position_player = 0;//玩家位置
        public int position_enemy = 0;//敌人位置
        public int energy = 0;
        public int hit = 0;
        public List<string> log; //记录
        public List<string> cardlist_ShouPai;//手牌卡牌
        public List<string> cardlist_QiPai;//弃牌卡牌
        public List<string> cardlist_PaiDui;//牌堆卡牌
        public int enemyHP = 0;
        public int enemySheild = 0;
        public int enemyState = 0;
        public int enemyInstruction1 = 0;//敌人指令1
        public int enemyInstruction2 = 0;//敌人指令2
    }


    //将当前游戏信息存入
    public void JsonWrite(float time, int round, int HP, int sheild, int state, int action, int position1, int position2, int energy, int hit, List<string> log, List<string> cardlist_ShouPai, List<string> cardlist_QiPai, List<string> cardlist_PaiDui, int enemyHP, int enemySheild, int enemyState, int enemyInstruction1, int enemyInstruction2)
    {
        GameStatus gameStatus;
        gameStatus = new GameStatus();
        gameStatus.time = time;
        gameStatus.round = round;
        gameStatus.HP = HP;
        gameStatus.sheild = sheild;
        gameStatus.state = state;
        gameStatus.action = action;
        gameStatus.position_player = position1;
        gameStatus.position_enemy = position2;
        gameStatus.energy = energy;
        gameStatus.hit = hit;
        gameStatus.log = log;
        gameStatus.cardlist_ShouPai = cardlist_ShouPai;
        gameStatus.cardlist_QiPai = cardlist_QiPai;
        gameStatus.cardlist_PaiDui = cardlist_PaiDui;
        gameStatus.enemyHP = enemyHP;
        gameStatus.enemySheild = enemySheild;
        gameStatus.enemyState = enemyState;
        gameStatus.enemyInstruction1 = enemyInstruction1;
        gameStatus.enemyInstruction2 = enemyInstruction2;
        string json = JsonUtility.ToJson(gameStatus);
        string savePath = Application.dataPath + "/Resources/data/LoadInfo.txt";
        File.WriteAllText(savePath, json, Encoding.UTF8);
        Debug.Log("save:::" + savePath);
    }

    //读取之前存储的游戏信息，返回一个GameStatus的对象
    public GameStatus JsonRead()
    {
        TextAsset textJson;
        GameStatus Info = null;
        textJson = Resources.Load("data/LoadInfo") as TextAsset;
        Info = JsonMapper.ToObject<GameStatus>(textJson.text);
        return Info;
    }


    // Use this for initialization
    void Start()
    {
        CardSet();
        /*
        nameset = NameSet();

        string card_pic_path;
        card_pic_path = Search(nameset[3], OriginalList);
        Debug.Log(nameset[3]);
        Debug.Log(card_pic_path);

        List<string> a = new List<string>();
        a.Add("AP001");
        a.Add("AP003");
        List<string> b = new List<string>();
        b.Add("AP004");
        b.Add("AP005");
        List<string> c = new List<string>();
        c.Add("AP003");
        c.Add("AP008");
        List<string> d = new List<string>();
        d.Add("AP007");
        d.Add("AP011");
        JsonWrite(0, 5, 4, 3, 2, 5, 3, 8, 5, 2, a, b, d, c, 3, 4, 5, 6, 8);
        */
    }//listInfo1,listinfo2中存有强化前后的牌组，数组形式。数据库文件存储在assets/resources中。
   

    // Update is called once per frame
    /* void Update () {

     }
     */


}