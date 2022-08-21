using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameResult
{
    public bool gameClear;
    public float activeTime; // targetTime보다 낮아야 합격
    public float targetTime;
    public int remainCount; // targetCount보다 낮아야 합격
    public int targetCount;

    public GameResult(bool clear, float at, float tt, int rc, int tc)
    {
        gameClear = clear;
        activeTime = at;
        targetTime = tt;
        remainCount = rc;
        targetCount = tc;
    }
}

public class InGameManager : Singleton<InGameManager>
{
    public GameObject[] Maps;
    public Player player;

    public float[] targetTimes = new float[3];
    public int[] targetCounts = new int[3];
    public int[] startCounts = new int[3];
    public int mapIdx;

    private void Awake()
    {
        mapIdx = PlayerPrefs.GetInt("idx", 0);
        for (int i = 0; i < Maps.Length; i++)
        {
            if (mapIdx == i) Maps[i].SetActive(true);
            else Maps[i].SetActive(false);
        }
    }

    public void GameClear()
    {
        PlayerStop();
        GameResult result = new GameResult(true, player.lifeTime, targetTimes[mapIdx], player.moveCount, targetCounts[mapIdx]);
        InGameUIManager.Instance.PrintResult(result);
    }

    public void GameOver()
    {
        PlayerStop();
        GameResult result = new GameResult(false, player.lifeTime, targetTimes[mapIdx], player.moveCount, targetCounts[mapIdx]);
        InGameUIManager.instance.PrintResult(result);
    }

    void PlayerStop()
    {
        player.rb.gravityScale = 0f;
        player.rb.velocity = Vector2.zero;
        player.state = PlayerState.StandBy;
        player.gameEnd = true;
    }
}
