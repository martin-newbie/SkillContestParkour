using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWholeResult : MonoBehaviour
{

    public Text[] stageScoresTxt = new Text[3];
    float[] stageScores = new float[3];

    public Text wholeScoreTxt;
    public float wholeScore;

    void Start()
    {
        stageScores = GameManager.Instance.StageScore;
        foreach (var item in stageScores)
        {
            wholeScore += item;
        }
        StartCoroutine(PrintResults());
    }

    IEnumerator PrintResults()
    {
        string[] formats = new string[3]
        {
            "Stage 1: ",
            "Stage 2: ",
            "Stage 3: "
        };

        for (int i = 0; i < stageScoresTxt.Length; i++)
        {
            yield return StartCoroutine(TextCounting(stageScoresTxt[i], stageScores[i], 1f, formats[i]));
            yield return new WaitForSeconds(1f);
        }
        yield return StartCoroutine(TextCounting(wholeScoreTxt, wholeScore, 1f, ""));
    }

    IEnumerator TextCounting(Text text, float target, float duration, string format)
    {
        float offset = target / duration;
        float timer = duration;
        float cur = 0f;

        while (timer > 0f)
        {
            text.text = string.Format(format + "{0:0,#}", (int)cur);
            cur += offset * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }

        cur = target;
        text.text = string.Format(format + "{0:0,#}", (int)cur);
    }
}
