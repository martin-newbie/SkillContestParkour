using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public Text ScoreTxt;
    public Text GameClearTxt;
    public Text RemainTimeTxt;
    public Text RemainInputTxt;

    public GameObject[] Stars;

    GameResult thisResult;

    public void Init(GameResult result)
    {
        thisResult = result;
        StartCoroutine(ResultCoroutine());
    }

    IEnumerator ResultCoroutine()
    {
        string gameClear = "Game Clear: " + thisResult.gameClear.ToString();
        string remainTime = "Active Time: " + string.Format("{0:0}s", thisResult.activeTime) + "/" + string.Format("{0:0}s", thisResult.targetTime);
        string remainCount = "Remain Input: " + thisResult.remainCount.ToString() + "/" + thisResult.targetCount.ToString();

        bool clear_0 = thisResult.gameClear;
        bool clear_1 = thisResult.activeTime <= thisResult.targetTime;
        bool clear_2 = thisResult.remainCount >= thisResult.targetCount;

        yield return StartCoroutine(PrintText(GameClearTxt, gameClear));
        yield return new WaitForSeconds(0.5f);
        GameClearTxt.color = clear_0 ? Color.green : Color.red;
        yield return new WaitForSeconds(0.5f);
        Stars[0].SetActive(clear_0);

        yield return StartCoroutine(PrintText(RemainTimeTxt, remainTime));
        yield return new WaitForSeconds(0.5f);
        RemainTimeTxt.color = clear_1 ? Color.green : Color.red;
        yield return new WaitForSeconds(0.5f);
        Stars[1].SetActive(clear_1);
        
        yield return StartCoroutine(PrintText(RemainInputTxt, remainCount));
        yield return new WaitForSeconds(0.5f);
        RemainInputTxt.color = clear_2 ? Color.green : Color.red;
        yield return new WaitForSeconds(0.5f);
        Stars[2].SetActive(clear_2);

        yield break;
    }

    IEnumerator PrintText(Text text, string content)
    {
        StringBuilder sb = new StringBuilder();
        int i = 0;
        while (i < content.Length)
        {
            sb.Append(content[i]);
            text.text = sb.ToString();
            yield return new WaitForSeconds(0.05f);
            i++;
        }

        yield break;
    }

    void Start()
    {
        
    }

}
