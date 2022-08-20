using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public Text ScoreTxt;
    public Text GameClearTxt;
    public Text RemainTimeTxt;
    public Text RemainInputTxt;

    public GameObject[] Stars;
    public GameObject Buttons;

    bool clear_0;
    bool clear_1;
    bool clear_2;

    GameResult thisResult;

    public void Init(GameResult result)
    {
        thisResult = result;
        StartCoroutine(ResultCoroutine());
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Next()
    {
        int idx = InGameManager.Instance.mapIdx;
        float extraScore = (clear_0 ? 1000 : 0) + (clear_1 ? 100 : 0) + (clear_2 ? 100 : 0);
        GameManager.Instance.StageScore[idx] = InGameManager.Instance.player.score + extraScore;

        if (idx < 2)
        {
            PlayerPrefs.SetInt("idx", idx + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // whole result
            // outro
        }
    }

    IEnumerator ResultCoroutine()
    {
        string gameClear = "Game Clear: " + thisResult.gameClear.ToString();
        string remainTime = "Active Time: " + string.Format("{0:0}s", thisResult.activeTime) + "/" + string.Format("{0:0}s", thisResult.targetTime);
        string remainCount = "Remain Input: " + thisResult.remainCount.ToString() + "/" + thisResult.targetCount.ToString();

        clear_0 = thisResult.gameClear;
        clear_1 = thisResult.activeTime <= thisResult.targetTime;
        clear_2 = thisResult.remainCount >= thisResult.targetCount;

        yield return StartCoroutine(PrintText(GameClearTxt, gameClear));
        yield return new WaitForSeconds(0.5f);
        GameClearTxt.color = clear_0 ? Color.green : Color.red;
        yield return new WaitForSeconds(0.5f);
        Stars[0].SetActive(clear_0);

        if (!clear_0)
        {
            clear_1 = false;
            clear_2 = false;
            goto Finish;
        }

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
        yield return new WaitForSeconds(1f);

        Finish:
        Buttons.SetActive(true);

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
