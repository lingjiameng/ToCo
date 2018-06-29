
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class gameManager : MonoBehaviour
{
    public List<DataManager.Card> listInfo1;//base卡
    public List<DataManager.Card> listInfo2;//advanced卡
    public GameObject self;// 调用己方人物的属性
    public GameObject enemy;//调用对方人物的属性

    public int round;
    //以下是己方回合可以调用的函数



    public void clearhit()//回合结束
    {
        self.GetComponent<effect_self>().hit = 0;
    }


    public bool can_forward()//判断能够前进
    {
        return ((self.GetComponent<effect_self>().Distance() > 0) &&
            self.GetComponent<effect_self>().moveEnergyValue > 0) &&
            (self.GetComponent<effect_self>().shield_Value < 7);
    }

    public int forward()//前进
    {
        self.GetComponent<effect_self>().move(1);//移动一个单位
        self.GetComponent<effect_self>().shield_Value++;//盾牌+1
        self.GetComponent<effect_self>().moveEnergyValue--;//行动力-1
        int distance = self.GetComponent<effect_self>().Distance();//返回双方相对距离
        return distance;

    }

    public bool can_backward()//判断能够后退
    {
        return ((self.GetComponent<effect_self>().Distance() < 10) &&
            self.GetComponent<effect_self>().moveEnergyValue > 0) &&
            self.GetComponent<effect_self>().shield_Value > 0;
    }

    public int backward()//后退
    {
        self.GetComponent<effect_self>().move(-1);//后退一个单位
        self.GetComponent<effect_self>().shield_Value--;//盾牌-1
        self.GetComponent<effect_self>().moveEnergyValue--;//行动力-1
        int distance = self.GetComponent<effect_self>().Distance();//返回双方相对距离
        return distance;

    }


    public void state_judge()//回合开始时判定状态
    {
        int self_state = self.GetComponent<effect_self>().state;

        if (self_state == 1)
        {
            self.GetComponent<effect_self>().AddshieldValue(3);
            self.GetComponent<effect_self>().state = 0;
        }
        //回盾

        if (self_state == 2)
        {
            self.GetComponent<effect_self>().AddselfBlood(-2);
            self.GetComponent<effect_self>().state = 0;
        }
        //扣血
        if (self.GetComponent<effect_self>().state == 3)
        {
            //抽卡2张
            self.GetComponent<playerControl>().AddNewCard(2);
            Debug.Log("add new card2!");
            self.GetComponent<effect_self>().state = 0;
        }
        if (self.GetComponent<effect_self>().state == 4)
        {
            //抽卡3张
            Debug.Log("add new card3!");
            self.GetComponent<playerControl>().AddNewCard(3);
            self.GetComponent<effect_self>().state = 0;
        }

    }


    public bool judge(string cardname)//判定base卡
    {
        DataManager.Card cardnum = Getcard(cardname);//将字符串转化为卡牌对象


        int distance = self.GetComponent<effect_self>().Distance();//获得当前的实际距离
        int selfHP = self.GetComponent<effect_self>().blood;//己方血量
        int selfshield = self.GetComponent<effect_self>().shield_Value;//己方盾牌
        int enemyHP = enemy.GetComponent<effect_enemy>().blood;//敌方血量
        int enemyshield = enemy.GetComponent<effect_enemy>().shield_Value;//敌方盾牌

        //判断是否可出


        //特例AP013
        if (cardname == "AP013")
            return distance <= 4 && distance >= 2;

        //其余
        bool distance_judge = true;
        bool HP_judge = true;
        bool shield_judge = true;

        //距离判断
        //Debug.Log(cardname );
        if (cardnum.distance_low > distance || cardnum.distance_high < distance)
            distance_judge = false;
        if (cardnum.distance_change > distance)
            distance_judge = false;

        //血量和盾牌判断


        if (cardnum.target == "enemy")
        {
            if (enemyHP == 10 && cardnum.healthrecovery > 0)
                HP_judge = false;
            if (enemyshield == 7 && cardnum.sheildbreak > 0)
                shield_judge = false;
        }
        else
        {
            if (selfHP == 10 && cardnum.healthrecovery > 0)
                HP_judge = false;
            if (selfshield == 7 && cardnum.sheildbreak > 0)
                shield_judge = false;
        }

        return (distance_judge && HP_judge) && shield_judge;//3个判定都为真才返回真


    }



    public DataManager.Card Getcard(string cardname)//将接收的卡牌序号转化为卡牌对象
    {

        for (int i = 0; i < listInfo1.Count; i++)
        {
            if (cardname == listInfo1[i].card_pic_name) return listInfo1[i];
        }
        for (int i = 0; i < listInfo2.Count; i++)
        {
            if (cardname == listInfo2[i].card_pic_name) return listInfo2[i];
        }
        return null;

    }

    public void one_card(string cardname)//发动base卡
    {
        DataManager.Card cardnum = Getcard(cardname);//将卡牌序号转化为卡牌类
        int self_state = self.GetComponent<effect_self>().state;
        int enemy_state = enemy.GetComponent<effect_enemy>().state;



        if (cardname == "AP013")
        {
            int distance = self.GetComponent<effect_self>().Distance();
            if (distance == 2)
            {
                enemy.GetComponent<effect_enemy>().AddshieldValue(-2);
                enemy.GetComponent<effect_enemy>().TakeDamage(1);
                enemy.GetComponent<effect_enemy>().AddshieldValue(-1);
                enemy.GetComponent<effect_enemy>().TakeDamage(2);
            }
            if (distance == 3)
            {
                enemy.GetComponent<effect_enemy>().AddshieldValue(-2);
                enemy.GetComponent<effect_enemy>().TakeDamage(1);
                enemy.GetComponent<effect_enemy>().AddshieldValue(-2);
                enemy.GetComponent<effect_enemy>().TakeDamage(2);
                enemy.GetComponent<effect_enemy>().AddshieldValue(-1);
                enemy.GetComponent<effect_enemy>().TakeDamage(2);

            }
            if (distance == 4)
            {
                enemy.GetComponent<effect_enemy>().AddshieldValue(-2);
                enemy.GetComponent<effect_enemy>().TakeDamage(2);
                enemy.GetComponent<effect_enemy>().AddshieldValue(-1);
                enemy.GetComponent<effect_enemy>().TakeDamage(2);

            }
            self.GetComponent<effect_self>().AddHit(cardnum.hit);


            return;
        }

        if (cardnum.ID == "AP004" || cardnum.ID == "AP005")
        {
            if (cardnum.ID == "AP004") enemy.GetComponent<effect_enemy>().AddshieldValue(-4);
            if (cardnum.ID == "AP005") { enemy.GetComponent<effect_enemy>().TakeDamage(2); self.GetComponent<effect_self>().move(1); }
            self.GetComponent<playerControl>().AddNewCard("AP005");
            return;
        }

        if (cardnum.ID == "AP010")
        {

            enemy.GetComponent<effect_enemy>().TakeDamage(self.GetComponent<effect_self>().hit);
            clearhit();
            return;

        }

        if (cardnum.ID == "AP007")
        {
            enemy.GetComponent<effect_enemy>().TakeDamage(self.GetComponent<effect_self>().Distance());
            return;
        }

        if (cardnum.ID == "AP008")
        {
            self.GetComponent<effect_self>().AddEnergyValue(cardnum.energygain);
            self.GetComponent<playerControl>().AddNewCard(1);
            return;
        }

        if (cardnum.ID == "AP011")
        {
            self.GetComponent<effect_self>().move(-2);
            self.GetComponent<playerControl>().AddNewCard(1);
            return;
        }

        int pre_selfblood = self.GetComponent<effect_self>().blood;
        int pre_enemyblood = enemy.GetComponent<effect_enemy>().blood;

        if (cardnum.target == "enemy")//卡牌发动对象是敌方
        {
            enemy.GetComponent<effect_enemy>().AddshieldValue(cardnum.sheildbreak);//对敌方的盾牌进行打击
            enemy.GetComponent<effect_enemy>().AddselfBlood(cardnum.healthrecovery);//对敌方的血量进行打击
            enemy.GetComponent<effect_enemy>().AddshieldValue(cardnum.sheildbreak);//一般打击（先盾牌判定，盾牌足够扣除盾牌，盾牌不够后扣除血量）
            enemy.GetComponent<effect_enemy>().move(cardnum.distance_change);//移动
            enemy.GetComponent<effect_enemy>().state = cardnum.state;//赋予状态

        }
        else//卡牌发动对象是己方
        {
            self.GetComponent<effect_self>().AddshieldValue(cardnum.sheildbreak);
            self.GetComponent<effect_self>().AddselfBlood(cardnum.healthrecovery);
            self.GetComponent<effect_self>().TakeDamage(cardnum.damage);
            self.GetComponent<effect_self>().move(cardnum.distance_change);
            self.GetComponent<effect_self>().AddEnergyValue(cardnum.energygain);
            self.GetComponent<effect_self>().state = cardnum.state;//赋予状态

        }
        int now_selfblood = self.GetComponent<effect_self>().blood;
        int now_enemyblood = enemy.GetComponent<effect_enemy>().blood;
        int change_enegry = ((now_enemyblood - pre_enemyblood) > 0 ? (now_enemyblood - pre_enemyblood) : (pre_enemyblood - now_enemyblood)) + ((now_selfblood - pre_selfblood) > 0 ? (now_selfblood - pre_selfblood) : (pre_selfblood - now_selfblood));
        self.GetComponent<effect_self>().AddEnergyValue(change_enegry);




    }

    public bool can_strengthen(string example)//判定强化卡
    {
        DataManager.Card cardnum = Getcard(example);//将卡牌序号转化为卡牌类

        int strength = self.GetComponent<effect_self>().energy;
        if (cardnum.level == "advance" && cardnum.energycost <= strength)
            if (judge(strengthen(cardnum).card_pic_name)) return true;
        return false;
    }

    public DataManager.Card strengthen(DataManager.Card example)//转换成强化卡
    {
        for (int i = 0; i < listInfo2.Count; i++)
        {
            if (example.ID == listInfo2[i].ID) return listInfo2[i];
        }
        /* for (int i = 0; i < listInfo1.Count; i++)
         {
             if (example.ID == listInfo1[i].ID) return listInfo1[i];
         }*/
        return null;

    }

    public void two_card(string cardname)//发动advance卡
    {
        DataManager.Card cardnum0 = Getcard(cardname);//将卡牌序号转化为卡牌类
        DataManager.Card cardnum = strengthen(cardnum0);//强化
        self.GetComponent<effect_self>().AddEnergyValue(-cardnum0.energycost);
        int self_state = self.GetComponent<effect_self>().state;
        int enemy_state = enemy.GetComponent<effect_enemy>().state;
        int pre_selfblood = self.GetComponent<effect_self>().blood;
        int pre_enemyblood = enemy.GetComponent<effect_enemy>().blood;

        if (cardnum.ID == "AP007")
        {
            enemy.GetComponent<effect_enemy>().TakeDamage(self.GetComponent<effect_self>().Distance() * 2);
            return;

        }
        if (cardnum.ID == "AP008")
        {
            self.GetComponent<effect_self>().AddEnergyValue(cardnum.energygain);
            self.GetComponent<playerControl>().AddNewCard(2);
            return;
        }
        if (cardnum.ID == "AP011")
        {
            self.GetComponent<effect_self>().move(-1);
            self.GetComponent<playerControl>().AddNewCard(3);
            return;
        }

        if (cardnum.target == "enemy")//卡牌发动对象是敌方
        {
            enemy.GetComponent<effect_enemy>().AddshieldValue(cardnum.sheildbreak);//对敌方的盾牌进行打击
            enemy.GetComponent<effect_enemy>().AddselfBlood(cardnum.healthrecovery);//对敌方的血量进行打击
            enemy.GetComponent<effect_enemy>().AddshieldValue(cardnum.sheildbreak);//一般打击（先盾牌判定，盾牌足够扣除盾牌，盾牌不够后扣除血量）
            enemy.GetComponent<effect_enemy>().move(cardnum.distance_change);//移动
            enemy.GetComponent<effect_enemy>().state = cardnum.state;//赋予状态

        }
        else//卡牌发动对象是己方
        {
            self.GetComponent<effect_self>().AddshieldValue(cardnum.sheildbreak);
            self.GetComponent<effect_self>().AddselfBlood(cardnum.healthrecovery);
            self.GetComponent<effect_self>().TakeDamage(cardnum.damage);
            self.GetComponent<effect_self>().move(cardnum.distance_change);
            self.GetComponent<effect_self>().AddEnergyValue(cardnum.energygain);
            self.GetComponent<effect_self>().state = cardnum.state;//赋予状态

        }
        int now_selfblood = self.GetComponent<effect_self>().blood;
        int now_enemyblood = enemy.GetComponent<effect_enemy>().blood;
        int change_enegry = ((now_enemyblood - pre_enemyblood) > 0 ? (now_enemyblood - pre_enemyblood) : (pre_enemyblood - now_enemyblood)) + ((now_selfblood - pre_selfblood) > 0 ? (now_selfblood - pre_selfblood) : (pre_selfblood - now_selfblood));
        self.GetComponent<effect_self>().AddEnergyValue(change_enegry);



    }

    public bool can_change()//判定弃牌
    {
        if (self.GetComponent<effect_self>().moveEnergyValue < 3)
            return true;
        return false;
    }


    public void change(string cardnum)//发动弃牌
    {
        Debug.Log(self.GetComponent<effect_self>().moveEnergyValue);
        self.GetComponent<effect_self>().moveEnergyValue += 1;
        return;
    }


    int Getaction()
    {
        int distance = self.GetComponent<effect_self>().Distance();
        int action = 0;
        if (distance >= 6) action = 1;
        else if (distance <= 3) action = 3;
        else action = 2;
        if (action == 1)
        {
            enemy.GetComponent<effect_enemy>().nextAttackA = enemy.GetComponent<effect_enemy>().position - 7;
            enemy.GetComponent<effect_enemy>().nextAttackB = enemy.GetComponent<effect_enemy>().position - 4;


        }
        else if (action == 2)
        {
            enemy.GetComponent<effect_enemy>().nextAttackA = enemy.GetComponent<effect_enemy>().position - 5;
            enemy.GetComponent<effect_enemy>().nextAttackB = enemy.GetComponent<effect_enemy>().position - 1;

        }
        else
        {
            enemy.GetComponent<effect_enemy>().nextAttackA = enemy.GetComponent<effect_enemy>().position - 2;
            enemy.GetComponent<effect_enemy>().nextAttackB = enemy.GetComponent<effect_enemy>().position - 1;

        }
        if (enemy.GetComponent<effect_enemy>().nextAttackA < 0) enemy.GetComponent<effect_enemy>().nextAttackA = 0;
        if (enemy.GetComponent<effect_enemy>().nextAttackB < 0) enemy.GetComponent<effect_enemy>().nextAttackB = 0;
        return action;


    }

    //boss1回合调用函数
    public void boss1_func()
    {

        int distance = self.GetComponent<effect_self>().Distance();
        if (Getaction() == 1)
        {
            int rangeRadomNum = Random.Range(0, 2);

            switch (rangeRadomNum)
            {
                case 0:
                    if (distance >= 4 && distance <= 7)
                        self.GetComponent<effect_self>().TakeDamage(4);
                    break;

                case 1:
                    enemy.GetComponent<effect_enemy>().move(-2);
                    //后退2
                    break;

            }

        }
        else if (Getaction() == 2)
        {
            int rangeRadomNum = Random.Range(0, 4);
            switch (rangeRadomNum)
            {
                case 0:
                    if (distance == 4)
                        self.GetComponent<effect_self>().TakeDamage(2);
                    break;
                case 1:
                    if (distance <= 5 && distance >= 1)
                        self.GetComponent<effect_self>().TakeDamage(1);
                    break;
                case 2:
                    self.GetComponent<effect_self>().moveEnergyValue = 0;
                    break;
                case 3:
                    enemy.GetComponent<effect_enemy>().move(2);
                    //前进2
                    break;

            }

        }
        else
        {
            int rangeRadomNum = Random.Range(0, 5);
            switch (rangeRadomNum)
            {
                case 0:
                    enemy.GetComponent<effect_enemy>().move(1);
                    //前进1；
                    break;
                case 1:
                    if (distance == 1 || distance == 2)
                        self.GetComponent<effect_self>().TakeDamage(4);
                    break;
                case 2:
                    enemy.GetComponent<effect_enemy>().AddselfBlood(3);
                    break;
                case 3:
                    enemy.GetComponent<effect_enemy>().AddshieldValue(2);
                    break;
                case 4:
                    enemy.GetComponent<effect_enemy>().move(-6);
                    //后退6
                    break;

            }

        }


    }

    public int Getsituation(int state = 0, int blood = 0)
    {

        if (state == 1 && blood <= 10) return 2;
        else if (state == 2 && blood >= 15) return 3;
        else return state;

    }

    public int boss2_state = 1;
    public int damage = 0;

    public void boss2_func()
    {
        int distance = enemy.GetComponent<effect_enemy>().Distance();
        int pre = boss2_state;
        boss2_state = Getsituation(boss2_state, enemy.GetComponent<effect_enemy>().blood);
        if (pre == 1 && boss2_state == 2)
        {
            enemy.GetComponent<effect_enemy>().blood = 5;
            enemy.GetComponent<effect_enemy>().shield_Value = 20;
            damage = 1;
        }
        if (pre == 2 && boss2_state == 3)
        {
            enemy.GetComponent<effect_enemy>().blood = 20;
            enemy.GetComponent<effect_enemy>().shield_Value = 0;
            damage = 2;
        }
        if (boss2_state == 1)
        {
            if (round == 2) { damage = 1; return; }
            int rangeRadomNum = Random.Range(0, 4);
            switch (rangeRadomNum)
            {
                case 0:
                    if (distance == 1 || distance == 3 || distance == 5 || distance == 7 || distance == 9)
                        enemy.GetComponent<effect_enemy>().TakeDamage(2 + damage);
                    self.GetComponent<effect_self>().moveEnergyValue = 0;
                    break;
                case 1:
                    if (distance == 2 || distance == 4 || distance == 6 || distance == 8)
                        self.GetComponent<effect_self>().TakeDamage(4 + damage);
                    break;
                case 2:
                    if (distance >= 2 && distance <= 7)
                        self.GetComponent<effect_self>().TakeDamage(1 + damage);
                    enemy.GetComponent<effect_enemy>().move(-2);
                    break;
                case 3:
                    if (distance >= 2 && distance <= 7)
                        self.GetComponent<effect_self>().TakeDamage(1 + damage);
                    enemy.GetComponent<effect_enemy>().move(2);
                    break;
            }

        }
        else if (boss2_state == 2)
        {
            if (round == 10)
            {
                enemy.GetComponent<effect_enemy>().AddselfBlood(-4);
                if (distance >= 0 && distance <= 4)
                    self.GetComponent<effect_self>().TakeDamage(8 + damage);

            }
            else if (round / 2 == 1)
            {
                enemy.GetComponent<effect_enemy>().AddselfBlood(2);
                enemy.GetComponent<effect_enemy>().AddshieldValue(3);
            }
            else
            {
                if (distance >= 0 && distance <= 4) self.GetComponent<effect_self>().TakeDamage(1 + damage);

            }


        }
        else
        {

            int rangeRadomNum = Random.Range(0, 4);
            switch (rangeRadomNum)
            {
                case 0:
                    if (distance == 1 || distance == 3 || distance == 5 || distance == 7 || distance == 9)
                        enemy.GetComponent<effect_enemy>().TakeDamage(2 + damage);
                    self.GetComponent<effect_self>().moveEnergyValue = 0;
                    break;
                case 1:
                    if (distance == 2 || distance == 4 || distance == 6 || distance == 8)
                        self.GetComponent<effect_self>().TakeDamage(4 + damage);
                    break;
                case 2:
                    if (distance >= 2 && distance <= 7)
                        self.GetComponent<effect_self>().TakeDamage(1 + damage);
                    enemy.GetComponent<effect_enemy>().move(-2);
                    break;
                case 3:
                    if (distance >= 2 && distance <= 7)
                        self.GetComponent<effect_self>().TakeDamage(1 + damage);
                    enemy.GetComponent<effect_enemy>().move(2);
                    break;
            }
            if (distance >= 0 && distance <= 10)
                self.GetComponent<effect_self>().TakeDamage(10 + damage);

        }


    }




    void Start()
    {
        listInfo1 = GameObject.Find("DataManager").GetComponent<DataManager>().OriginalList;
        listInfo2 = GameObject.Find("DataManager").GetComponent<DataManager>().IntensifiedList;
        self = GameObject.Find("self");
        enemy = GameObject.Find("Enemy");


    }

    // Update is called once per frame
    void Update()
    {
        listInfo1 = GameObject.Find("DataManager").GetComponent<DataManager>().OriginalList;
        listInfo2 = GameObject.Find("DataManager").GetComponent<DataManager>().IntensifiedList;
        round = self.GetComponent<playerControl>().round;
    }




}







