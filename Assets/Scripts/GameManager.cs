using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    // 클리어 or 게임오버 팝업창
    [SerializeField] private GameObject popupCanvas;
    // 게임 클리어 여부
    private bool isCleared;
    public bool IsCleared { get { return isCleared; } }


    private PlayerController playerController;
    private Player player;
    private LifeDisplayer LifeDisplayer;
    private int life = 5;

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
    {   // 생명 이미지 활성화
        LifeDisplayer.SetLives(life);
    }

    void Update()
    {

    }

    // 데미지 받으면 생명 감소 후 UI반영
    public void Damage()
    {
        if (life > 0) 
        {
            life--;
            LifeDisplayer.SetLives(life);
        }
        else
        {
            GameOver();
        }
    }

    // 재시작
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // 게임오버
    void GameOver()
    {
        isCleared = false;
        popupCanvas.SetActive(true);
    }
    // 게임 클리어
    public void GameClear()
    {
        isCleared = true;
        popupCanvas.SetActive(true);
    }


}
