using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour {
    // Use this for initialization

    public GameObject Panel_settings;
    public GameObject Panel_achievement; //Achievement
    public GameObject Panel_map;
    public GameObject Panel_cards;

    public GameObject Button_up;
    public GameObject Image_right;
    public GameObject Image_mid;
    public GameObject Image_left;

    private Vector3 mouse;
    private Vector3[] corners;
    private bool token;

    public static bool LoadData;
    public static bool isOpenMap = false;

    void Awake()
    {
        if (Panel_settings) Panel_settings.SetActive(false);
        if (Panel_achievement) Panel_achievement.SetActive(false);
        if (Panel_map) Panel_map.SetActive(false);
        if (Panel_cards) Panel_cards.SetActive(false);
        corners = new Vector3[4];
        token = false;
    }

    void Start ()
    {
        LoadData = false;
        if (isOpenMap)
        {
            OpenMap();
            isOpenMap = false;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (!token)
        {
            return;
        }

        Image Image_Map = Panel_map.GetComponent<Image>();
        Sprite sp;



        mouse = Input.mousePosition;
        // mouse = Camera.main.WorldToScreenPoint(transform.position);
        float x = mouse.x;
        float y = mouse.y;
        float z = mouse.z;

        Button_up.GetComponent<RectTransform>().GetWorldCorners(corners);
        //左下、左上、右上、右下

        if (corners[0].x < x && corners[0].y < y &&
            corners[1].x < x && corners[1].y > y &&
            corners[2].x > x && corners[2].y > y &&
            corners[3].x > x && corners[3].y < y)
        {
            //Debug.Log("in_up");
            sp = Resources.Load("image/MainUI/highlight_up", typeof(Sprite)) as Sprite;
            Image_Map.sprite = sp;
            return;
        }

        Image_left.GetComponent<RectTransform>().GetWorldCorners(corners);
        //左下、左上、右上、右下

        if (corners[0].x < x && corners[0].y < y &&
            corners[1].x < x && corners[1].y > y &&
            corners[2].x > x && corners[2].y > y &&
            corners[3].x > x && corners[3].y < y)
        {
            //Debug.Log("in_left");
            sp = Resources.Load("image/MainUI/highlight_left", typeof(Sprite)) as Sprite;
            Image_Map.sprite = sp;
            return;
        }

        Image_right.GetComponent<RectTransform>().GetWorldCorners(corners);
        //左下、左上、右上、右下

        if (corners[0].x < x && corners[0].y < y &&
            corners[1].x < x && corners[1].y > y &&
            corners[2].x > x && corners[2].y > y &&
            corners[3].x > x && corners[3].y < y)
        {
            //Debug.Log("in_right");
            sp = Resources.Load("image/MainUI/highlight_right", typeof(Sprite)) as Sprite;
            Image_Map.sprite = sp;
            return;
        }

        Image_mid.GetComponent<RectTransform>().GetWorldCorners(corners);
        //左下、左上、右上、右下

        if (corners[0].x < x && corners[0].y < y &&
            corners[1].x < x && corners[1].y > y &&
            corners[2].x > x && corners[2].y > y &&
            corners[3].x > x && corners[3].y < y)
        {
            //Debug.Log("in_mid");
            sp = Resources.Load("image/MainUI/highlight_mid", typeof(Sprite)) as Sprite;
            Image_Map.sprite = sp;
            return;
        }

        //Debug.Log("out");
        sp = Resources.Load("image/MainUI/map", typeof(Sprite)) as Sprite;
        Image_Map.sprite = sp;


    }

    public void Settings_OnClick(){
        Panel_settings.SetActive(true);
    }
    public void BackSettings_OnClick()
    {
        Panel_settings.SetActive(false);
    }

    public void Achievement_OnClick()
    {
        Panel_achievement.SetActive(true);
    }
    public void BackAchievement_OnClick()
    {
        Panel_achievement.SetActive(false);
    }

    public void Map_OnClick()
    {
        LoadData = false;
        Panel_map.SetActive(true);
        token = true;
    }
    public void BackMap_OnClick()
    {
        Panel_map.SetActive(false);
        token = false;
    }

    public void Cards_OnClick()
    {
        Panel_cards.SetActive(true);
    }
    public void BackCards_OnClick()
    {
        Panel_cards.SetActive(false);
    }
    public void ContinueGame()
    {
        LoadData = true;
        SceneManager.LoadScene("battle");
    }

    public void OpenMap()
    {
        if (Panel_map) Panel_map.SetActive(true);
        token = true;
        LoadData = false;
    }
    public void GameQuit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
