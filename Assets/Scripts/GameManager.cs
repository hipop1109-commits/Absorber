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
    // 게임 클리어 여부
    private bool isCleared;
    public bool IsCleared { get { return isCleared; } }
   
    [SerializeField]private LifeDisplayer lifeDisplayer;
    // 생명 수 
    [SerializeField]private int life = 10; 

    private Player player;

    // 저장된 데이터 구조
    [Serializable]
    public class SaveData
    {
        public int playerLife;
        public Vector3 playerPosition;
    }


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
            player = new Player(life, 0f,0f,0f);
            lifeDisplayer.SetLives(player.PlayerHp, player.PlayerMaxHp);
            
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
            //player.TakeDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            player.Heal(1);
        }

    }
    // 저장 데이터 저장
    public static void SaveGame(int slot)
    {
        SaveData data = new SaveData
        {
             // 예시 데이터
            playerLife = 10,
            playerPosition = new Vector3(0, 0, 0)
            
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveSlot" + slot, json);
        PlayerPrefs.Save();
        Debug.Log("Game Saved in Slot " + slot);
    }
    // 저장 데이터 불러오기
    public static SaveData LoadGame(int slot)
    {
        string key = "SaveSlot" + slot;
        if (PlayerPrefs.HasKey(key))
        {
            string json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<SaveData>(json);
        }
        Debug.LogWarning("No Save Data in Slot " + slot);
        return null;
    }

    // 슬롯 상태 확인
    public static bool IsSlotEmpty(int slot)
    {
        return !PlayerPrefs.HasKey("SaveSlot" + slot);
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
