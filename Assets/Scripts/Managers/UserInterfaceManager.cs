using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    // Singleton pointer
    public static UserInterfaceManager Instance { get; private set; }

    [SerializeField]
    private Canvas inGameUI = null;

    [SerializeField]
    private Canvas gameOverScreen = null;

    private void Awake()
    {
        // Set this class as Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        inGameUI.GetComponentInChildren<Text>().text = ScoreManager.Score.ToString();
    }

    public void DisplayGameOverScreen()
    {
        gameOverScreen.gameObject.SetActive(true);
    }

    public void HideGameOverScreen()
    {
        gameOverScreen.gameObject.SetActive(false);
    }

    public void DisplayInGameUI()
    {
        inGameUI.gameObject.SetActive(true);
    }

    public void HideInGameUI()
    {
        inGameUI.gameObject.SetActive(false);
    }

}
