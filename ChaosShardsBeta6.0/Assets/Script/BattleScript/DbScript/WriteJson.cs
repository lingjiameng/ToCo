using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using LitJson;

[System.Serializable]
public class WriteJson : MonoBehaviour
{

    public int token = 123;

    public class GameStatus
    {
        public int time = 0;
        public int round = 0;
        public int HP = 0;
        public int sheild = 0;
        public int state = 0;
        public int action = 0;
        public int position1 = 0;
        public int position2 = 0;
        public int energy = 0;
        public int hit = 0;
        public List<string> log; //记录
        public List<string> cardlist1;//手牌卡牌
        public List<string> cardlist2;//弃牌卡牌
        public List<string> cardlist3;//牌堆卡牌
        public int enemyHP = 0;
        public int enemySheild = 0;
        public int enemyState = 0;
        public int enemyInstruction1 = 0;
        public int enemyInstruction2 = 0;
    }


    public GameStatus gameStatus;

    public void JsonWrite(int time, int round, int HP, int sheild, int state, int action, int position1,int position2, int energy,int hit, List<string> log, List<string> l1, List<string> l2, List<string> l3,int enemyHP,int enemySheild,int enemyState,int enemyInstruction1,int enemyInstruction2)
    {
        Debug.Log("B");
        GameStatus gameStatus;
        gameStatus = new GameStatus();
        gameStatus.time = time;
        gameStatus.round = round;
        gameStatus.HP = HP;
        gameStatus.sheild = sheild;
        gameStatus.state = state;
        gameStatus.action = action;
        gameStatus.position1 = position1;
        gameStatus.position2 = position2;
        gameStatus.energy = energy;
        gameStatus.hit = hit;
        gameStatus.log = log;
        gameStatus.cardlist1 = l1;
        gameStatus.cardlist2 = l2;
        gameStatus.cardlist3 = l3;
        gameStatus.enemyHP = enemyHP;
        gameStatus.enemySheild = enemySheild;
        gameStatus.enemyState = enemyState;
        gameStatus.enemyInstruction1 = enemyInstruction1;
        gameStatus.enemyInstruction2 = enemyInstruction2;
        string json = JsonUtility.ToJson(gameStatus);
        //Application.persistentDataPath=Application.dataPath + "/Resources/data/LoadInfo.txt";
        string savePath = Application.persistentDataPath+ "/LoadInfo.txt";//Application.dataPath + "/Resources/data/LoadInfo.txt";
        File.WriteAllText(savePath, json, Encoding.UTF8);
        Debug.Log("json::" + json);
        Debug.Log("save:::" + savePath);

        Debug.Log("C");
    }

    public GameStatus JsonRead()
    {
        //TextAsset textJson;
        GameStatus Info = null;
        //textJson = File.ReadAllText(Application.persistentDataPath + "/LoadInfo.txt") as TextAsset;
        if(File.Exists(Application.persistentDataPath + "/LoadInfo.txt"))
        {
            Info = JsonMapper.ToObject<GameStatus>(File.ReadAllText(Application.persistentDataPath + "/LoadInfo.txt"));
        }
        //Debug.Log("--===============")
        //Debug.Log(Info.cardlist1.Count);
        return Info;
    }

    void Start()
    {
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
        //JsonWrite(0,5,4,3,2,5,3,8,5,2,a,b,d,c,3,4,5,6,8);

       /* GameStatus Status;
        Status=JsonRead();
        Debug.Log(Status.sheild);*/
    }

    public void SaveJson()
    {
        
    }
}
/*[System.Serializable]
public class refencenes
{
    public refencenes()
    {
        name = "";
        id = -1;
    }

    public string name;
    public int id;
}
*/
