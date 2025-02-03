using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton <GameManager>
{
    private bool isCleared;
    public bool IsCleared { get { return isCleared; } }
   
    [SerializeField] private LifeDisplayer lifeDisplayer;
    
    [SerializeField] private int life = 10; 

    private Player player;
    private PlayerController playerController;

    [SerializeField] GameObject gameOverPanel;    
    

    private void Start()
    {   
        if (lifeDisplayer != null)
        {
            player = new Player(life, 0f,0f,0f);
            lifeDisplayer.SetLives(player.PlayerHp, player.PlayerMaxHp);
            
        }

        playerController = FindObjectOfType<PlayerController>();

    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    //player.TakeDamage(1);
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    player.Heal(1);
        //}

    }
    // 게임 오버 패널 활성화
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    // 메인 메뉴 이동
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    // 게임 종료
    public void QuitGame()
    {
        Application.Quit();
    }

    // 재시작
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
