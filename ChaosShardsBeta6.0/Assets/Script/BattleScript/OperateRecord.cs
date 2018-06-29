using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperateRecord : MonoBehaviour
{

    public GameObject self;
    public GameObject enemy;
    public GameObject timer;

    public int position_self;
    public int position_enemy;
    public int hit;

    public float time;
    public int round;

    public int state_self;
    public int shield_self;
    public int blood_self;
    public int action_self;
    public int energy_self;

    public int state_enemy;
    public int nextAttack_enemyA, nextAttack_enemyB;
    public int shield_enemy;
    public int blood_enemy;

    public List<string> log; //记录
    public List<string> cardlist1;//手牌卡牌
    public List<string> cardlist2;//弃牌卡牌
    public List<string> cardlist3;//牌堆卡牌

    public GameObject RecordManager;

    // Use this for initialization
    void Start()
    {
        self = GameObject.Find("self");
        enemy = GameObject.Find("Enemy");
        timer = GameObject.Find("runTime");
        RecordManager = GameObject.Find("RecordManager");
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    // 存档
    public void SaveDataRecord()
    {
        Debug.Log("开始存档");
        time = timer.GetComponent<timer>().TimeCounter;
        round = self.GetComponent<playerControl>().round;
        position_self = self.GetComponent<effect_self>().position;
        position_enemy = enemy.GetComponent<effect_enemy>().position;
        state_self = self.GetComponent<effect_self>().state;
        shield_self = self.GetComponent<effect_self>().shield_Value;
        blood_self = self.GetComponent<effect_self>().blood;
        shield_enemy = enemy.GetComponent<effect_enemy>().shield_Value;
        blood_enemy = enemy.GetComponent<effect_enemy>().blood;
        action_self = self.GetComponent<effect_self>().moveEnergyValue;
        energy_self = self.GetComponent<effect_self>().energy;
        nextAttack_enemyA = enemy.GetComponent<effect_enemy>().nextAttackA;
        nextAttack_enemyB = enemy.GetComponent<effect_enemy>().nextAttackB;
        hit = self.GetComponent<effect_self>().hit;
        state_enemy = enemy.GetComponent<effect_enemy>().state;

        cardlist1 = self.GetComponent<playerControl>().CardList;
        cardlist2 = self.GetComponent<playerControl>().UsedCardList;
        cardlist3 = self.GetComponent<playerControl>().CardHeapList;
        Debug.Log("Token" + " A");
        //public void JsonWrite(float time, int round, int HP, int sheild, int state, int action, int position1, int position2, int energy, int hit, List<string> log, List<string> l1, List<string> l2, List<string> l3, int enemyHP, int enemySheild, int enemyState, int enemyInstruction1, int enemyInstruction2)
        RecordManager.GetComponent<WriteJson>().JsonWrite((int)time, round, blood_self, shield_self, state_self, action_self, position_self, position_enemy, energy_self, hit, log, cardlist1, cardlist2, cardlist3, blood_enemy, shield_enemy, state_enemy, nextAttack_enemyA, nextAttack_enemyB);
        Debug.Log("结束存档");
    }

    // 读出的变量
    public WriteJson.GameStatus info;


    // 读档
    public bool ReadDataRecord()
    {
        self.GetComponent<playerControl>().LoadState();

        info = RecordManager.GetComponent<WriteJson>().JsonRead();
        if (info == null) return false;
        Debug.Log("Token B");
        Debug.Log(info.time);

        timer.GetComponent<timer>().TimeCounter = (int)info.time;
        Debug.Log("Compare  " + timer.GetComponent<timer>().TimeCounter);
        self.GetComponent<playerControl>().round = info.round;
        //self.GetComponent<effect_self>().position = info.position1;
        //enemy.GetComponent<effect_enemy>().position = info.position2;

        self.GetComponent<effect_self>().state = info.state;
        self.GetComponent<effect_self>().shield_Value = info.sheild;
        self.GetComponent<effect_self>().blood = info.HP;
        enemy.GetComponent<effect_enemy>().shield_Value = info.enemySheild;
        enemy.GetComponent<effect_enemy>().blood = info.enemyHP;
        self.GetComponent<effect_self>().moveEnergyValue = info.action;
        self.GetComponent<effect_self>().energy = info.energy;
        enemy.GetComponent<effect_enemy>().nextAttackA = info.enemyInstruction1;
        enemy.GetComponent<effect_enemy>().nextAttackB = info.enemyInstruction2;
        self.GetComponent<effect_self>().hit = info.hit;
        enemy.GetComponent<effect_enemy>().state = info.enemyState;


        self.GetComponent<effect_self>().position = 1;
        enemy.GetComponent<effect_enemy>().position = 11;
        Debug.Log("---------------------------------");
        Debug.Log(info.position1);
        Debug.Log(info.position2);
        self.GetComponent<effect_self>().move(info.position1 - 1);
        enemy.GetComponent<effect_enemy>().move(11 - info.position2);

        Debug.Log("Token C");
        self.GetComponent<playerControl>().LoadCard(info.cardlist3, info.cardlist1, info.cardlist2);
        return true;
    }
}
