using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Properties
    // Singleton pointer
    public static ScoreManager Instance { get; private set; }

    private static bool isScoreStopped = true;
    public static bool IsScoreStopped { get { return isScoreStopped; } }

    private static int score = 0;
    public static int Score
    {
        get { return score; }
    }

    #endregion

    #region Functions
    private void Awake()
    {
        // Set this class as Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public static void IncreaseScore()
    {
        if (isScoreStopped)
            return;

        // Increase scroe
        score += 5;

        // Spawn bomb hexagon in every 1000 score
        if (score % 1000 == 0)
            RespawnManager.Instance.SpawnBombHexagon();
            
    }

    public static void Start()
    {
        isScoreStopped = true;
    }

    public static void StartScore()
    {
        isScoreStopped = false;
    }

    public static void StopScore()
    {
        isScoreStopped = true;
    }

    public static void ResetScore()
    {
        score = 0;
    }
    #endregion
}
