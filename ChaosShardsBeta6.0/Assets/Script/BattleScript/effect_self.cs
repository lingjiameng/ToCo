using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class effect_self : MonoBehaviour
{
    public int hit;     // 记录hit数量
    public int energy;  // 记录能量值

    public int state;

    public Slider bloodSlider; // 血条
    public int blood;
    public const int BLOOD_MAX = 10;

    public Text shield;
    public int shield_Value = 5; // 盾牌
    public const int SHIELD_VALUE_MAX = 7;

    public GameObject Enemy;
    public GameObject self;

    // 现在r1-r6是无用的接口，可以用来调试
    public Text r1;
    public Text r2;
    public Text r3;
    public Text r4;
    public Text r5;
    public Text r6;

    public Text HitText;

    public int r1Value = 0;
    public int r2Value = 0;
    public int r3Value = 0;
    public int r4Value = 0;
    public int r5Value = 0;
    public int r6Value = 0;

    // 行动力
    public int moveEnergyValue = 0;
    public Text moveEnergy;

    public int position; //  位置
    
    // 血条颜色改变的临界值
    public const int yellow = 6;
    public const int red = 3;

    // 地砖
    public GameObject tiles;


    // 粒子特效使用的游戏物体对象
    public GameObject fire;
    public GameObject tmp;

    // Use this for initialization
    void Start()
    {
            state = 0;
            energy = 0;
            hit = 0;
            position = 1;
            // 开始游戏 满血
            blood = BLOOD_MAX;



        // 获取游戏对象
        tiles = GameObject.Find("DistanceTiles");
        self = GameObject.Find("self");
        //Debug.Log("A");
        //tmp = Instantiate(fire, self.transform.position, self.transform.rotation);
        //Debug.Log("B" + tmp.name);
    }

    // Update is called once per frame
    void Update()
    {
        bloodSlider.value = blood;

        shield.text = shield_Value.ToString() + "/" + SHIELD_VALUE_MAX.ToString();

        r1.text = r1Value.ToString();
        r2.text = r2Value.ToString();
        r3.text = r3Value.ToString();
        r4.text = r4Value.ToString();
        r5.text = r5Value.ToString();
        r6.text = r6Value.ToString();

        HitText.text = hit.ToString();
        // hit UI

        moveEnergy.text = moveEnergyValue.ToString();
        
        // 动态改变血条颜色
        Transform fillTsf = bloodSlider.fillRect;

        if (bloodSlider.value < red)
        {
            fillTsf.GetComponent<Image>().color = Color.red;
        }
        else if (bloodSlider.value < yellow)
        {
            fillTsf.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            fillTsf.GetComponent<Image>().color = Color.green;
        }
    }

    /* 玩家受到攻击
     * 返回类型
     * 0  玩家 继续游戏
     * 1  玩家 游戏结束
     */
    public bool TakeDamage(int damage)
    {
        if (shield_Value >= damage)
        {
            // 盾牌掉血
            shield_Value -= damage;
        }
        else
        {
            // 人物掉血
            blood -= damage;
            //tmp = Instantiate(fire, self.transform.position, self.transform.rotation);
        }

        return (blood <= 0);
    }

    // 回血 盾牌
    public void AddshieldValue(int addValue)
    {
        shield_Value += addValue;
        shield_Value = Mathf.Min(SHIELD_VALUE_MAX, shield_Value);
        shield_Value = Mathf.Max(0, shield_Value);
    }
     
    /* 玩家回血函数 也可以用作掉血
     * 增加 或者 减少 玩家的血量
     * true  表示敌人死亡 游戏结束
     * false 表示敌人存活 游戏继续
     */
    public bool AddselfBlood(int addValue)
    {
        if (blood + addValue < 0)
        {
            blood = 0;
            return true;
        }
        blood = Mathf.Min(BLOOD_MAX, blood + addValue);
        return false;
    }

    // 移动 函数 不会出现非法position情况
    public void move(int moveDistance)
    {
        Debug.Log("MOVE");
        Debug.Log(position);
        Debug.Log(moveDistance);

        int realMoveDistance;

        if (moveDistance == 0)
            return;

        if (moveDistance > 0)
        {
            realMoveDistance = Mathf.Min(moveDistance, 12 - position);
        }
        else
        {
            realMoveDistance = Mathf.Max(moveDistance, 0 - position);
        }
        position += realMoveDistance;
        tiles = GameObject.Find("DistanceTiles");
        tiles.GetComponent<distance>().Move_Self(realMoveDistance);
    }

    public void Position(int sp)
    {
        position = sp;
        
    }

    // 获取相对距离 
    public int Distance()
    {
        return System.Math.Abs(Enemy.GetComponent<effect_enemy>().position - position);
    }

    /* 已失效
     * 传参分别为六个资源增加的值
     * 消耗资源传入负值
     * 当消耗资源时有一个资源不足就会返回false
     * 
     */
    public bool IncreaseResource(int addr1, int addr2, int addr3, int addr4, int addr5, int addr6)
    {
        if (r1Value + addr1 < 0 || r2Value + addr2 < 0 || r3Value + addr3 < 0 || r4Value + addr4 < 0 || r5Value + addr5 < 0 || r6Value + addr6 < 0)
            return false;
        r1Value += addr1;
        r2Value += addr2;
        r3Value += addr3;
        r4Value += addr4;
        r5Value += addr5;
        r6Value += addr6;
        return true;
    }

    //  只有使用行动力 传参为负 并且 行动力不足时才会返回 false
    public bool IncreaseMoveEnergy(int addEnergy)
    {
        if (moveEnergyValue + addEnergy < 0)
            return false;
        moveEnergyValue += addEnergy;
        moveEnergyValue = Mathf.Min(3, moveEnergyValue);
        return true;
    }

    // 增加hit值
    // 返回值是修改后的hit的数量
    public int AddHit(int addhit)
    {
        hit = hit + addhit;
        return hit;
    }
     
    public void AddEnergyValue(int addEnergy)
    {
        energy = energy + addEnergy;
    }
}
