using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : Singleton<InGameUIManager>
{
    public Text remainMoveCount;
    public Image[] remainLifeCount;
    public Text scoreTxt;
    public Text timeTxt;

    public Result resultUI;
    
    public void PrintResult(GameResult result)
    {
        resultUI.gameObject.SetActive(true);
        resultUI.Init(result);
    }

    public void SetScore(object score)
    {
        scoreTxt.text = "Score: " + score.ToString();
    }

    public void SetTime(float time)
    {
        string min = ((int)time / 60).ToString();
        string sec = string.Format("{0:0}", time % 60f);
        timeTxt.text = min + ":" + sec;
    }

    public void SetLife(int lifeCount)
    {
        for (int i = 0; i < remainLifeCount.Length; i++)
        {
            if(i < lifeCount)
            {
                remainLifeCount[i].gameObject.SetActive(true);
            }
            else
            {
                remainLifeCount[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetMoveCount(int count)
    {
        remainMoveCount.text = count.ToString();
    }
}
