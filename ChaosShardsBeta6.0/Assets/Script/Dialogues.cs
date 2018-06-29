using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.Collections.Generic;//用到了容器类  
using UnityEngine.SceneManagement;

public class Dialogues : MonoBehaviour
{

    //这是场景中的各个物体
    public GameObject roleA;
    public GameObject roleB;

    public GameObject left;
    public GameObject right;

    //private GameObject roleC;
    //private GameObject roleD;
    //private GameObject roleE;
    public GameObject roleName;
    public GameObject detail;

    //存放dialogues的list
    private List<string> dialogues_talk_role_list;
    private List<string> dialogues_list;
    private List<string> dialogues_role_left;
    private List<string> dialogues_role_right;

    private int dialogue_index = 0;//对话索引
    private int dialogue_count = 0;//对话数量

    private string role;//当前在说话的角色
    private string role_detail;//当前在说话的内容。


    public string DialoguesName;

    public Canvas canvas;
    public string background;

    void Start()
    {

        //变量初始化
        DialoguesName = NewSceneName.SceneName;

        Debug.Log("AAAA" + DialoguesName + DialoguesName.Length.ToString());

        if (DialoguesName == "dialogues")
        {
            background = "VenusBlood";
        }
        else
        {
            background = "battlebg2";
        }
        canvas.GetComponent<Image>().sprite = Resources.Load("talkData/" + background, typeof(Sprite)) as Sprite;

        dialogues_talk_role_list = new List<string>();
        dialogues_list = new List<string>();
        dialogues_role_left = new List<string>();
        dialogues_role_right = new List<string>();

        roleA = GameObject.Find("Canvas/roleA");
        roleB = GameObject.Find("Canvas/roleB");

        //roleE = GameObject.Find("Canvas/roleE");
        //roleC = GameObject.Find("Canvas/roleC");
        //roleD = GameObject.Find("Canvas/roleD");

        //roleName = GameObject.Find("Canvas/Dialogues/roleName");
        //detail = GameObject.Find("Canvas/Dialogues/detail");

        //roleA.SetActive(true);
        //roleB.SetActive(true);
        XmlDocument xmlDocument = new XmlDocument();//新建一个XML“编辑器”  
        dialogues_list = new List<string>();//初始化存放dialogues的list
        //载入资源文件
        string data = Resources.Load("talkData\\" + DialoguesName).ToString();//注意这里没有后缀名xml。你可以看看编辑器中，也是不带后缀的。因此不要有个同名的其它格式文件注意！
        //如果Resources下又有目录那就：Resources.Load("xx\\xx\\dialogues").ToString()

        Debug.Log(data);

        xmlDocument.LoadXml(data);//载入这个xml  

        //选择<dialogues>为根结点并得到旗下所有子节点 
        XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("dialogues").ChildNodes;//选择<dialogues>为根结点并得到旗下所有子节点  

        foreach (XmlNode xmlNode in xmlNodeList)//遍历<dialogues>下的所有节点<dialogue>压入List
        {
            XmlElement xmlElement = (XmlElement)xmlNode;//对于任何一个元素，其实就是每一个<dialogue>  
            Debug.Log(xmlElement.ChildNodes.Item(0).InnerText);
            dialogues_talk_role_list.Add(xmlElement.ChildNodes.Item(0).InnerText); // 说话角色
            dialogues_list.Add(xmlElement.ChildNodes.Item(3).InnerText); // 内容
            dialogues_role_left.Add(xmlElement.ChildNodes.Item(1).InnerText);    //  左边人物
            dialogues_role_right.Add(xmlElement.ChildNodes.Item(2).InnerText);   // 右边人物
            //将角色名和对话内容存入这个list，中间存个逗号一会儿容易分割
        }
        dialogue_count = dialogues_list.Count;//获取到底有多少条对话
        dialogues_handle(0);//载入第一条对话的场景
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))//如果点击了鼠标左键
        {
            dialogue_index++;//对话跳到一下个
            if (dialogue_index < dialogue_count)//如果对话还没有完
            {
                dialogues_handle(dialogue_index);//那就载入下一条对话
            }
            else
            {
                //对话完了
                //进入下一游戏场景之类的
                if (DialoguesName == "dialogues")
                {
                    SceneManager.LoadScene("battle");
                }
                else
                {
                    SceneManager.LoadScene("battle2");
                } 
            }
        }
    }

    /*处理每一条对话的函数，就是将dialogues_list每一条对话弄到场景*/
    private void dialogues_handle(int dialogue_index)
    {
        //切割数组
        string role_detail = dialogues_list[dialogue_index];
        string leftrole = dialogues_role_left[dialogue_index];
        string rightrole = dialogues_role_right[dialogue_index];
        string rolename = dialogues_talk_role_list[dialogue_index];

        Debug.Log("LOG");
        Debug.Log(leftrole);

        if (leftrole == "")
        {
            left.SetActive(false);
            //left.GetComponent<Image>().sprite = Resources.Load("talkData/empty", typeof(Sprite)) as Sprite;
        }
        else
        {
            left.SetActive(true);
            string str;
            str = "talkData/" + leftrole;
            Debug.Log(str);
            left.GetComponent<Image>().sprite = Resources.Load("talkData/" + leftrole, typeof(Sprite)) as Sprite;
        }

        if (rightrole == "")
        {
            right.SetActive(false);
            right.GetComponent<Image>().sprite = Resources.Load("talkData/empty", typeof(Sprite)) as Sprite;

        }
        else
        {
            right.SetActive(true);
            right.GetComponent<Image>().sprite = Resources.Load("talkData/" + rightrole, typeof(Sprite)) as Sprite;
        }

        roleName.GetComponent<Text>().text = rolename;

        // 对话人物
        roleName.GetComponent<typewriter>().Initialize();
        // 对话内容
        detail.GetComponent<Text>().text = role_detail;//并加载当前的对话
        //GameObject.Find("脚本所在的物体的名字").GetComponent<脚本名>().函数名();
        detail.GetComponent<typewriter>().Initialize();
    }

}
