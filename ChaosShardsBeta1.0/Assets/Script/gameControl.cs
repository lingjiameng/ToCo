using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameControl : MonoBehaviour {

    private GameObject PlayerSelf;
    private GameObject Enemy;
    public GameObject GameEndPanel;
    public GameObject timer;

    public Text GameResult;
    public Text GameTime;
    public Text GameRound;

    public Text RunTime;
    public Text Round;

    private bool token;

    enum PlayerStateForEnd {
        playing,
        win,
        lose
    };

    PlayerStateForEnd PlayerState = PlayerStateForEnd.playing;

    // Use this for initialization
    void Start () {
        PlayerSelf = GameObject.Find("self");
        Enemy = GameObject.Find("Enemy");
        timer = GameObject.Find("runTime");
        GameEndPanel.SetActive(false);

        GameResult.text = "游戏结果: ";
        GameTime.text = "游戏时间: ";
        GameRound.text = "游戏回合: ";
        token = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerSelf.GetComponent<effect_self>().blood <= 0)
        {
            PlayerState = PlayerStateForEnd.lose;
        }
        else if(Enemy.GetComponent<effect_enemy>().blood <= 0)
        {
            PlayerState = PlayerStateForEnd.win;
        }

        if (PlayerState == PlayerStateForEnd.playing)
            return;

        DisplayPanel();
    }


    void DisplayPanel()
    {
        if (token)
            return;

        timer.GetComponent<timer>().Stop_Run();
        // 游戏结束
        PlayerSelf.GetComponent<playerControl>().myState = playerControl.GameStageSate.GameEnd;

        GameTime.text = GameTime.text + RunTime.text;
        GameRound.text = GameRound.text + Round.text[1];
        GameEndPanel.SetActive(true);

        if (PlayerState == PlayerStateForEnd.win)
        {
            // Win
            GameResult.text = GameResult.text + "你赢了！！！";

        }
        else
        {
            // Lose
            GameResult.text = GameResult.text + "你输了QAQ";
        }
        token = true;
    }

}
