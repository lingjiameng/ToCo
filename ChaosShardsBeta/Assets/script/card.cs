using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card : MonoBehaviour {

    private Image image;
    private string cardName;
    private int cardId;

    //获取控制对象
    private GameObject playerSelf;
    private GameObject gameFunc;

    //下面为卡牌状态
    private bool isSelected=false;//默认未选中
    //private static int selectNum=0;
    
    void Awake()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// 初始化图片
    /// </summary>
    /// <param name="cardInfo"></param>
    public void InitImage(string card_Name)
    {
        this.cardName = card_Name;
        image.sprite = Resources.Load("image/cards/deck/" + cardName, typeof(Sprite)) as Sprite;
    }

    // Use this for initialization
    void Start () {
        playerSelf = GameObject.Find("self");
        gameFunc = GameObject.Find("GameManager");

        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 设置选择状态//每次使用先判断是否 已经点击 或者是否 可点击，再调用setState
    /// </summary>
    public void SetSelectState()
    {
        if (isSelected)
        {   //卡牌复位
            iTween.MoveTo(gameObject, transform.position - Vector3.up * 10f, 1f);
            this.GetComponent<Transform>().localScale = Vector3.one * 1f;//图片大小还原
        }
        else
        {
            //选中卡牌
            iTween.MoveTo(gameObject, transform.position + Vector3.up * 10f, 1f);
            this.GetComponent<Transform>().localScale = Vector3.one * 1f; //图片两倍放大
        }

        isSelected = !isSelected;
    }
    public bool SelectState()
    {
        return isSelected;
    }

    public void OnClick()
    {
        if (isSelected || isUseful())
        {
            SetSelectState();
        }
    }

    public bool isUseful()
    {
        return (playerSelf.GetComponent<playerControl>().myState == playerControl.GameStageSate.Wait || 
            playerSelf.GetComponent<playerControl>().myState == playerControl.GameStageSate.DisCardWait)&&
            gameFunc.GetComponent<gameManager>().judge(cardName);//检查阶段是否正确
        //检查是否有其他牌被选中
        //检查距离 和 能量
        //return true;
    }
}
