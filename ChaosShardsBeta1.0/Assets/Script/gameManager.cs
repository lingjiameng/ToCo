using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public class card
    {
        //卡片的12个属性
        public string name;
        public string ID;
        public string type;
        public string target;
        public string level;
        public int healthrecover;
        public int sheildbreak;
        public int damage;
        public int distance_low;
        public int distance_high;
        public int distance_change;
        public string detail;
        //卡片的构造函数
        public card(string n, string I, string ty, string tar, string l, int he, int sh, int d, int dis_low, int dis_high, int dis_change, string det)
        {
            name = n;
            ID = I;
            type = ty;
            target = tar;
            level = l;
            healthrecover = he;
            sheildbreak = sh;
            damage = d;
            distance_low = dis_low;
            distance_high = dis_high;
            distance_change = dis_change;
            detail = det;
        }
    }



    public GameObject self;// 调用己方人物的属性
    public GameObject enemy;//调用对方人物的属性

    public bool judge(string cardname)//根据接受的卡牌序号判断卡牌是否可以出手
    {
        card cardnum = Getcard(cardname);//将字符串转化为卡牌对象
        if (cardnum.ID == "AP000") return false;
        int distance = self.GetComponent<effect_self>().Distance();//获得当前的实际距离
        int selfHP = self.GetComponent<effect_self>().blood;//己方血量
        int selfshield = self.GetComponent<effect_self>().shield_Value;//己方盾牌
        int enemyHP = enemy.GetComponent<effect_enemy>().blood;//敌方血量
        int enemyshield = enemy.GetComponent<effect_enemy>().shield_Value;//敌方盾牌
        //判断是否可出
        bool distance_judge = true;
        bool HP_judge = true;
        bool shield_judge = true;
        //距离判断
        if (cardnum.distance_low > distance || cardnum.distance_high < distance)
            distance_judge = false;
        if (cardnum.distance_change > distance)
            distance_judge = false;

        //血量和盾牌判断
        if (cardnum.target == "enemy")
        {
            if (enemyHP == 10 && cardnum.healthrecover > 0)
                HP_judge = false;
            if (enemyshield == 7 && cardnum.sheildbreak > 0)
                shield_judge = false;
        }
        else
        {
            if (selfHP == 10 && cardnum.healthrecover > 0)
                HP_judge = false;
            if (selfshield == 7 && cardnum.sheildbreak > 0)
                shield_judge = false;
        }
        return (distance_judge && HP_judge) && shield_judge;//3个判定都为真才返回真
    }

    public bool judge(card cardnum)//根据卡牌对象判断是否可出
    {
        int distance = self.GetComponent<effect_self>().Distance();//获得当前的实际距离
        int selfHP = self.GetComponent<effect_self>().blood;//己方血量
        int selfshield = self.GetComponent<effect_self>().shield_Value;//己方盾牌
        int enemyHP = enemy.GetComponent<effect_enemy>().blood;//敌方血量
        int enemyshield = enemy.GetComponent<effect_enemy>().shield_Value;//敌方盾牌
        //判断是否可出
        bool distance_judge = true;
        bool HP_judge = true;
        bool shield_judge = true;
        //距离判断
        if (cardnum.distance_low > distance || cardnum.distance_high < distance)
            distance_judge = false;

        //血量和盾牌判断
        if (cardnum.target == "enemy")
        {
            if (enemyHP == 10 && cardnum.healthrecover < 0)
                HP_judge = false;
            if (enemyshield == 7 && cardnum.sheildbreak < 0)
                shield_judge = false;
        }
        else
        {
            if (selfHP == 10 && cardnum.healthrecover < 0)
                HP_judge = false;
            if (selfshield == 7 && cardnum.sheildbreak < 0)
                shield_judge = false;
        }
        return (distance_judge && HP_judge) && shield_judge;//3个判定都为真才返回真
    }


    public card Getcard(string cardname)//将接收的卡牌序号转化为卡牌对象
    {
        card card0 = new card("非法卡牌", "AP000", "wrong", "wrong", "wrong", 0, 0, 0, 0, 0, 0, "非法卡牌");
        card card1 = new card("斩击", "AP001", "attack", "enemy", "base", 0, 0, 2, 0, 10, 0, "造成2点伤害");
        card card2 = new card("疾风步", "AP002", "attack", "self", "base", 0, 0, 0, 0, 10, 2, "距离-2");
        card card3 = new card("回复药", "AP003", "attack", "self", "base", 2, 0, 0, 0, 10, 0, "回复自己2点生命值");
        card card4 = new card("格挡", "AP004", "attack", "self", "base", 0, 2, 0, 0, 10, 0, "护盾+2");
        card card5 = new card("起风", "AP005", "effect", "self", "base", 0, 0, -2, 0, 10, 0, "可转化为advance卡牌");
        card card6 = new card("风灵之息", "AP006", "attack", "enemy", "advance", -5, -5, 0, 0, 10, 0, "可转化为advance卡牌");


        if (card1.ID == cardname) return card1;
        if (card2.ID == cardname) return card2;
        if (card3.ID == cardname) return card3;
        if (card4.ID == cardname) return card4;
        if (card5.ID == cardname) return card5;
        if (card6.ID == cardname) return card6;
        return card0;

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



    public void one_card(string cardname)//根据接收到的卡牌序号来进行该卡牌的判定
    {
        card cardnum = Getcard(cardname);//将卡牌序号转化为卡牌类
        if (cardnum.ID == "AP000") return;
        if (cardnum.target == "enemy")//卡牌发动对象是敌方
        {
            enemy.GetComponent<effect_enemy>().AddshieldValue(cardnum.sheildbreak);//对敌方的盾牌进行打击
            enemy.GetComponent<effect_enemy>().AddselfBlood(cardnum.healthrecover);//对敌方的血量进行打击
            enemy.GetComponent<effect_enemy>().TakeDamage(cardnum.damage);//一般打击（先盾牌判定，盾牌足够扣除盾牌，盾牌不够后扣除血量）
            enemy.GetComponent<effect_enemy>().move(cardnum.distance_change);//移动
        }
        else//卡牌发动对象是己方
        {
            self.GetComponent<effect_self>().AddshieldValue(cardnum.sheildbreak);
            self.GetComponent<effect_self>().AddselfBlood(cardnum.healthrecover);
            self.GetComponent<effect_self>().TakeDamage(cardnum.damage);
            self.GetComponent<effect_self>().move(cardnum.distance_change);

        }
    }


    void one_card(card cardnum)
    {
        if (cardnum.target == "enemy")
        {
            enemy.GetComponent<effect_enemy>().AddshieldValue(cardnum.sheildbreak);
            enemy.GetComponent<effect_enemy>().AddselfBlood(cardnum.healthrecover);
            enemy.GetComponent<effect_enemy>().TakeDamage(cardnum.damage);
            enemy.GetComponent<effect_enemy>().move(cardnum.distance_change);
        }
        else
        {
            self.GetComponent<effect_self>().AddshieldValue(cardnum.sheildbreak);
            self.GetComponent<effect_self>().AddselfBlood(cardnum.healthrecover);
            self.GetComponent<effect_self>().TakeDamage(cardnum.damage);
            self.GetComponent<effect_self>().move(cardnum.distance_change);

        }


    }

    void Start()
    {
        self = GameObject.Find("self");
        enemy = GameObject.Find("Enemy");


    }

    // Update is called once per frame
    void Update()
    {

    }




}
