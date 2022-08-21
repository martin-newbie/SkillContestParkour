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

    }

    IEnumerator TextCounting(Text text, float target, float duration, string format)
    {
        float offset = target / duration;
        float timer = duration;
        float cur = 0f;

        while (timer > 0f)
        {
            text.text = string.Format(format + "{0:0,#}", cur);
            cur += offset;
            yield return null;
        }

        cur = target;
        text.text = string.Format(format + "{0:0,#}", cur);
    }
}
