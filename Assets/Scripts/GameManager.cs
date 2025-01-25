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

    // �����?������ ����
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
    {   // ���� �̹��� Ȱ��ȭ
        if (lifeDisplayer != null)
        {
            player = new Player(life, 0f,0f,0f);
            lifeDisplayer.SetLives(player.PlayerHp, player.PlayerMaxHp);
            
        }
        else 
        {
            Debug.Log("������ ���÷��� Ȱ��ȭ �ȵƾ��?");
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
    // ���� ������ ����
    public static void SaveGame(int slot)
    {
        SaveData data = new SaveData
        {
             // ���� ������
            playerLife = 10,
            playerPosition = new Vector3(0, 0, 0)
            
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveSlot" + slot, json);
        PlayerPrefs.Save();
        Debug.Log("Game Saved in Slot " + slot);
    }
    // ���� ������ �ҷ�����
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

    // ���� ���� Ȯ��
    public static bool IsSlotEmpty(int slot)
    {
        return !PlayerPrefs.HasKey("SaveSlot" + slot);
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
