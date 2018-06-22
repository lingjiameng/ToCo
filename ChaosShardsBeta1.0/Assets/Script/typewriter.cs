using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class typewriter: MonoBehaviour
{

    public float charsPerSecond = 0.005f;//打字时间间隔
    private string words;//保存需要显示的文字

    private bool isActive = false;
    private float timer;//计时器
    private Text myText;
    private int currentPos = 0;//当前打字位置

    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        timer = 0;
        currentPos = 0;
        myText = GetComponent<Text>();
        words = myText.text;
        myText.text = "";
        // Start!
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        OnStartWriter();
        //Debug.Log (isActive);
    }

    public void StartEffect()
    {
        isActive = true;
    }
    /// <summary>
    /// 执行打字任务
    /// </summary>
    void OnStartWriter()
    {

        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= charsPerSecond)
            {//判断计时器时间是否到达
                timer = 0;
                currentPos++;

                if (currentPos >= words.Length)
                {
                    OnFinish();
                    return;
                }

                myText.text = words.Substring(0, currentPos);//刷新文本显示内容
            }

        }
    }
    /// <summary>
    /// 结束打字，初始化数据
    /// </summary>
    void OnFinish()
    {
        isActive = false;
        timer = 0;
        currentPos = 0;
        myText.text = words;
    }




}