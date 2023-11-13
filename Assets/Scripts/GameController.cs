using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public bool canClick;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        canClick = true;
    }

    public void CheckWin()
    {
        //if (!BoardController.instance.IsHasACouple())
        //{
        //    //stateGame = G12_StateGame.GameOver;
        //    if (score >= targetScore && stateGame == G12_StateGame.GameOver)
        //    {
        //        G12_PlayerPrefs.LevelIndex += 1;
        //        G12_CanvasController.instance.ShowWinPopup();
        //    }
        //    else
        //    {
        //        canClick = false;
        //        LoseGame();
        //    }
        //}
        //Debug.Log(stateGame);
        if (!BoardController.instance.IsHasACouple())
        {
            Debug.Log("End Game");
        }    

    }
}
