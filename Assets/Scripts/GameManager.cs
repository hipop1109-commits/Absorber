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
   
    [SerializeField]private LifeDisplayer lifeDisplayer;
    // 생명 수 
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
    {   // 생명 이미지 활성화
        if (lifeDisplayer != null)
        {
            player = new Player(life, 0f,0f,0f, lifeDisplayer);
            lifeDisplayer.SetLives(player.PlayerHp);
            
        }
        else 
        { 
            Debug.Log("라이프 디스플레이 활성화 안됐어요"); 
        }
    }

    void Update()
    {
        // 테스트용
        if (Input.GetKeyDown(KeyCode.G))
        {
<<<<<<< Updated upstream
            player.TakeDamage(1);
=======
            //player.TakeDamage(1);
            TestDamage();
>>>>>>> Stashed changes
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            player.Heal(1);
        }

    }

    void TestDamage()
    {
        if(life > 0)
        {
            life--;
            lifeDisplayer.SetLives(life);
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
