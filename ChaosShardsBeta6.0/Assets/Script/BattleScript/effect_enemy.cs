using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class effect_enemy : MonoBehaviour
{
    public Slider bloodSlider;
    public int blood;
    public int BLOOD_MAX = 10;

    public int nextAttackA, nextAttackB;

    public int state;

    public Text shield; // 盾
    public int shield_Value = 5; // 盾牌
    public  int SHIELD_VALUE_MAX = 7;

    public GameObject player;

    public const int yellow = 6;
    public const int red = 3;

    public int position = 11; //  位置

    public GameObject tiles;

    public GameObject Enemy;

    // Use this for initialization
    void Start()
    {
        position = 11;
        blood = BLOOD_MAX;
        tiles = GameObject.Find("DistanceTiles");
    }

    // Update is called once per frame
    void Update()
    {
        bloodSlider.value = blood;

        shield.text = shield_Value.ToString() + "/" + SHIELD_VALUE_MAX.ToString();

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


    /*
     * 增加 或者 减少 盾牌的血量
     * 盾牌 血量 0 - 7
     */
    public void AddshieldValue(int addValue)
    {
        shield_Value += addValue;
        shield_Value = Mathf.Min(SHIELD_VALUE_MAX, shield_Value);
        shield_Value = Mathf.Max(0, shield_Value);
    }

    /*
     * 增加 或者 减少 敌人的血量
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

    /*
     * 返回类型
     * 0  敌人 继续游戏
     * 1  敌人 游戏结束
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
        }

        return (blood <= 0);
    }

    public void move(int moveDistance)
    {
        tiles = GameObject.Find("DistanceTiles");

        int realMoveDistance;

        if (moveDistance == 0)
            return;

        if (moveDistance > 0)
        {
            realMoveDistance = Mathf.Min(moveDistance, position);
        }
        else
        {
            realMoveDistance = Mathf.Max(moveDistance, position - 12);
        }

        position -= realMoveDistance;
        
        tiles.GetComponent<distance>().Move_Enemy(realMoveDistance);

        //iTween.MoveTo(Enemy, iTween.Hash("position", tiles[position].GetComponent<Transform>().position, "time", 1f, "easetype", iTween.EaseType.easeInOutBack));
    }

    public int Distance()
    {
        return System.Math.Abs(player.GetComponent<effect_self>().position - position);
    }
}
