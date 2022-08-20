using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public float[] StageScore = new float[3];
    public float WholeScore
    {
        get
        {
            float score = 0f;
            foreach (var item in StageScore)
            {
                score += item;
            }
            return score;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

}
