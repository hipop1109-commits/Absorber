using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // 플레이어 생명 이미지
    public List<GameObject> lifeImages; 
    // 플레이어 생명 수
    [SerializeField] private int life = 5; 
    // 클리어 or 게임오버 팝업창
    [SerializeField] private GameObject popupCanvas;
    // 게임 클리어 여부
    private bool isCleared;
    public bool IsCleared { get { return isCleared; } }

    private PlayerController player;

    private static UIController instance;
    public static UIController Instance { get { return instance; } }

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
        SetLives(life);
    }
    void Update()
    {

    }

    // 사망 시 생명 감소 후 UI반영, 재시작 
    public void Die()
    {
        life--;
        SetLives(life);

        StartCoroutine(RestartCoroutine());
    }
    // 2초 후 재시작 
    private IEnumerator RestartCoroutine()
    {
        yield return new WaitForSeconds(2);
        Restart();
    }

    // 생명 이미지 활성화, 비활성화
    public void SetLives(int life)
    {
        for (int i = 0; i < lifeImages.Count; i++)
        {
            if (i < life)
            {
                lifeImages[i].SetActive(true);
            }
            else
            {
                lifeImages[i].SetActive(false);
            }
        }
    }
    // 재시작
    void Restart()
    {
        if (life > 0)
        {

        }
        else
        {
            GameOver();
        }
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
