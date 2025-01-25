using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    // 클리어 or 게임오버 팝업창
    [SerializeField] private GameObject popupCanvas;
    
    private bool isCleared;
    public bool IsCleared { get { return isCleared; } }
   
    [SerializeField]private LifeDisplayer lifeDisplayer;
    
    [SerializeField]private int life = 10; 

    private Player player;
    private PlayerController playerController;
    private SaveManager saveManager;

    [SerializeField] private Vector3 saveTargetPositon; // 자동 저장 트리거 위치
    [SerializeField] private float saveTriggerRadius = 1f; // 자동 저장 트리거 반경


    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null) 
        { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {   
        if (lifeDisplayer != null)
        {
            player = new Player(life, 0f,0f,0f);
            lifeDisplayer.SetLives(player.PlayerHp, player.PlayerMaxHp);
            
        }
        saveManager = FindObjectOfType<SaveManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            //player.TakeDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            player.Heal(1);
        }

        if (Vector3.Distance(playerController.transform.position, saveTargetPositon) <= saveTriggerRadius)
        {
            Debug.Log("Auto Save Triggered");
            saveManager.SaveGame();
        } 

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            saveManager.SaveGame();
        }
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
