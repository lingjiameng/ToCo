using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class card : MonoBehaviour
{

    private Image image;
    private string cardName;
    private string cardPath;
    private int cardId;
    private Vector3 org_pos;
    private float scale = 2f;

    //获取控制对象
    private GameObject playerSelf;
    private GameObject gameFunc;
    private GameObject btnPanel;
    private GameObject cardPanel;
    private GameObject cardProm;

    //获取鼠标位置
    private Vector3 RawMousePosition;
    //下面为卡牌状态
    private bool isSelected = false;//默认未选中
    //private static int selectNum=0;
    private bool isHover = false;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// 初始化图片
    /// </summary>
    /// <param name="cardInfo"></param>
    public void InitImage(string card_name, string card_path)
    {
        this.cardName = card_name;
        this.cardPath = card_path;
        image.sprite = Resources.Load(card_path, typeof(Sprite)) as Sprite;
    }

    // Use this for initialization
    void Start()
    {
        playerSelf = GameObject.Find("self");
        gameFunc = GameObject.Find("GameManager");
        btnPanel = GameObject.Find("BtnPanel");
        cardPanel = GameObject.Find("CardPanel");
        cardProm = GameObject.Find("CardPromotion");

        Button btn = this.GetComponent<Button>();

        btn.onClick.AddListener(OnClick);

        //btn.OnPointerEnter()

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnGUI()
    {
        RawMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    /// <summary>
    /// 设置选择状态//每次使用先判断是否 已经点击 或者是否 可点击，再调用setState
    /// </summary>
    public void SetSelectState()
    {
        if (isSelected)
        {
            //卡牌复位
            //iTween.MoveTo(gameObject, transform.position - Vector3.up * 10f, .1f);
            this.GetComponent<Transform>().localScale = Vector3.one * 1f;//图片大小还原

            //调整卡牌显示层级
            // transform.SetAsLastSibling();
            // transform.SetAsFirstSibling();  
            // transform.SetSiblingIndex(2);
        }
        else
        {
            //选中卡牌
            //iTween.MoveTo(gameObject, transform.position + Vector3.up * 10f, .1f);
            this.GetComponent<Transform>().localScale = Vector3.one * scale; //图片两倍放大


            //调整卡牌显示层级
            transform.SetAsLastSibling();
            // transform.SetAsFirstSibling();  
            // transform.SetSiblingIndex(2);
        }

        isSelected = !isSelected;
    }
    public bool SelectState()
    {
        return isSelected;
    }

    public void OnClick()
    {
        /*
        if (isSelected || isUseful())
        {
            SetSelectState();
        }
        */
    }

    public bool isUseful()
    {
        //检查能否打出
        Debug.Log(cardName);
        return (playerSelf.GetComponent<playerControl>().myState == playerControl.GameStageSate.Wait &&
            gameFunc.GetComponent<gameManager>().judge(cardName)) ||
            playerSelf.GetComponent<playerControl>().myState == playerControl.GameStageSate.DisCardWait;//检查阶段是否正确
        //检查是否有其他牌被选中
        //检查距离 和 能量
        //return true;
    }

    public void ChangeImage(string card_name, string card_path)
    {
        this.cardName = card_name;
        this.cardPath = card_path;
        name = cardName;
        image.sprite = Resources.Load(card_path, typeof(Sprite)) as Sprite;
    }

    //////////////////////
    //    2018-06-23    //
    //////////////////////

    public void OnMouseEnter()
    {
        //选中卡牌
        //iTween.MoveTo(gameObject, transform.position + Vector3.up * 10f, .1f);
        this.GetComponent<Transform>().localScale = Vector3.one * scale; //图片两倍放大
        isHover = true;

        //调整间距
        playerSelf.GetComponent<playerControl>().AdjPosition(cardName);
        //调整卡牌显示层级
        transform.SetAsLastSibling();
    }

    public void OnMouseExit()
    {
        //卡牌复位
        //iTween.MoveTo(gameObject, transform.position - Vector3.up * 10f, .1f);
        this.GetComponent<Transform>().localScale = Vector3.one * 1f;//图片大小还原
        isHover = false;
        //调整间距
        playerSelf.GetComponent<playerControl>().AdjPosition();
        //调整卡牌显示层级
        playerSelf.GetComponent<playerControl>().AdjLayer();

        // transform.SetAsLastSibling();
        // transform.SetAsFirstSibling();  
        // transform.SetSiblingIndex(2);
    }

    public void OnMouseDown()
    {
        //Debug.Log("just for test");

        Debug.Log("down");
    }

    public void OnMouseUP()
    {
        //Debug.Log("up");
    }

    public void OnBeginDrag()
    {
        Debug.Log("begin drag");
        org_pos = transform.position;
    }

    public void OnDrag()
    {
        if (!isHover) return;
        transform.position = RawMousePosition + Vector3.down * GetComponent<RectTransform>().rect.height * 0.45f * scale;
        //Debug.Log(transform.position);
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //Debug.Log("over");
    }

    public void OnEndDrag()
    {
        if (!isHover) return;
        if (btnPanel == null)
        {
            btnPanel = GameObject.Find("BtnPanel");
        }

        if (btnPanel && btnPanel.GetComponent<BoxCollider2D>().bounds.Contains(RawMousePosition))
        {
            //移入移动区，转化为行动力
            //检查是否能使用
            if(gameFunc.GetComponent<gameManager>().can_change())
            {
                Debug.Log("trans to move energy");
                gameFunc.GetComponent<gameManager>().change(cardName);
                playerSelf.GetComponent<playerControl>().DisCard(cardName);
            }
            else
            {
                //Debug.Log("trans to move energy");
                iTween.MoveTo(gameObject, org_pos, 1f);
            }
            return;


        }

        if (cardProm && cardProm.GetComponent<BoxCollider2D>().bounds.Contains(RawMousePosition))
        {
            //移入强化区，使用升级效果打出卡牌
            //检查是否能使用
            Debug.Log("will card promotion");
            if (gameFunc.GetComponent<gameManager>().can_strengthen(cardName))
            {
                Debug.Log("card promotion");
                gameFunc.GetComponent<gameManager>().two_card(cardName);
                playerSelf.GetComponent<playerControl>().DisCard(cardName);
            }
            else
            {
                //Debug.Log("card promotion");
                iTween.MoveTo(gameObject, org_pos, 1f);
                //return;
            }
            return;

        }

        if (cardPanel.GetComponent<BoxCollider2D>().bounds.Contains(RawMousePosition))
        {
            //在牌组区释放
            //移动回牌组，取消操作
            Debug.Log("dis_selected");
            iTween.MoveTo(gameObject, org_pos, 1f);
            return;
        }

        //正常使用卡牌
        //检查是否能使用
        //iTween.MoveTo(gameObject, org_pos, 1f);
        if (isUseful())
        {
            if (playerSelf.GetComponent<playerControl>().myState == playerControl.GameStageSate.DisCardWait)
            {
                Debug.Log("dis card");
                playerSelf.GetComponent<playerControl>().DisCard(cardName);
                return;
            }
            else
            {
                Debug.Log("use card");
                playerSelf.GetComponent<playerControl>().UseCard(cardName);
                return;
            }
        }
        else
        {
            //移动回牌组，取消操作
            Debug.Log("dis_selected");
            iTween.MoveTo(gameObject, org_pos, 1f);
            return;
        }
    }

    /*
    public void OnDrop()
    {
        Debug.Log("drop in" +name);
    }
    */
}
