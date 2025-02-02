using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton <GameManager>
{
    [SerializeField] private GameObject popupCanvas;
    
    private bool isCleared;
    public bool IsCleared { get { return isCleared; } }
   
    [SerializeField] private LifeDisplayer lifeDisplayer;
    
    [SerializeField] private int life = 10; 

    private Player player;
    private PlayerController playerController;
    

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

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void GameOver()
    {
        isCleared = false;
        popupCanvas.SetActive(true);
    }
    
    public void GameClear()
    {
        isCleared = true;
        popupCanvas.SetActive(true);
    }


}
