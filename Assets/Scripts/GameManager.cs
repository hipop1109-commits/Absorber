using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    // Ŭ���� or ���ӿ��� �˾�â
    [SerializeField] private GameObject popupCanvas;
    // ���� Ŭ���� ����
    private bool isCleared;
    public bool IsCleared { get { return isCleared; } }
   
    [SerializeField]private LifeDisplayer lifeDisplayer;
    // ���� �� 
    [SerializeField]private int life = 10; 

    private Player player;

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
    {   // ���� �̹��� Ȱ��ȭ
        if (lifeDisplayer != null)
        {
            player = new Player(life, 0f,0f,0f);
            lifeDisplayer.SetLives(player.PlayerHp, player.PlayerMaxHp);
            
        }
    }

    void Update()
    {
        // �׽�Ʈ��
        if (Input.GetKeyDown(KeyCode.G))
        {
            //player.TakeDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            player.Heal(1);
        }

    }
    
    // �����?
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // ���ӿ���
    void GameOver()
    {
        isCleared = false;
        popupCanvas.SetActive(true);
    }
    // ���� Ŭ����
    public void GameClear()
    {
        isCleared = true;
        popupCanvas.SetActive(true);
    }


}
