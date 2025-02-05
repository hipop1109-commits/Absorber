using System;
using System.Collections;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : Singleton<SaveManager>
{
    [Serializable]
    public class SaveData
    {
        public float playerX;
        public float playerY;
        public int playerHP;
        public int energyCore;
        public string lastSavedTime;
        public string currentScene;
    }

    private string savePath;
    private PlayerController playerController;
    private Player player;
    private MenuDisplayer menuDisplayer;
    private bool justLoaded = false; // 게임 로드 직후 세이브 방지 

    [SerializeField] private GameObject saveMessage;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/saveSlot.json"; // 저장 경로 설정
    }
    // 게임 세이브
    public void SaveGame()
    {
        if (justLoaded)
        {
            Debug.Log("자동저장 5초 후");
            return;
        }
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        SaveData saveData = new SaveData
        {
            playerX = playerController.transform.position.x,
            playerY = playerController.transform.position.y,
            playerHP = playerController.player.PlayerHp,
            energyCore = playerController.player.EnergyCore,
            lastSavedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            currentScene = SceneManager.GetActiveScene().name
        };

        string json = JsonUtility.ToJson(saveData); // 데이터를 JSON 형식으로 변환
        File.WriteAllText(savePath, json);  // 파일로 저장

        Debug.Log("Game Saved: " + savePath); // 저장 로그 출력

        menuDisplayer = FindFirstObjectByType<MenuDisplayer>();

        if (saveMessage != null) // 세이브 메세지 출력
            StartCoroutine(ShowSaveMessage());

        StartCoroutine(DontAutoSave());
    }
    // 세이브 메세지 출력
    private IEnumerator ShowSaveMessage()
    {
        saveMessage.SetActive(true);
        yield return new WaitForSeconds(1f);
        saveMessage.SetActive(false);
    }
    // 게임로드
    public SaveData LoadGame()
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<SaveData>(json); // JSON을 객체로 변환
        }

        Debug.LogWarning("세이브 파일없어요");
        return null;
    }

    // 클릭시 게임 로드
    public void LoadGameData(SaveData saveData)
    {   
        if(saveData != null)
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (saveData.currentScene != currentScene)
            {
                SceneManager.LoadScene(saveData.currentScene); // 다른 씬이면 해당 씬으로 변경
                StartCoroutine(WaitAndLoadData(saveData)); 
            }
            else
            {
                ApplySaveData(saveData);
            }
        }
    }
    // 씬 변경 후 데이터 로드
    private IEnumerator WaitAndLoadData(SaveData saveData)
    {
        // 씬이 로드될 때까지 기다림
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == saveData.currentScene);
        yield return new WaitForSeconds(0.1f);
       

        while (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
        ApplySaveData(saveData);
    }

    // 세이브 데이터 적용
    private void ApplySaveData(SaveData saveData)
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();
        // 세이브 데이터 적용
        playerController.player.RoadPlayerHp(saveData.playerHP);
        playerController.player.RoadEnergyCore(saveData.energyCore);
        playerController.transform.position = new Vector3(saveData.playerX, saveData.playerY, 0);
        // UI 업데이트
        LifeDisplayer.Instance.SetLives(playerController.player.PlayerHp, playerController.player.PlayerMaxHp);
        // 게임 로드 직후 일정 시간동안 자동 저장 방지
        StartCoroutine(DontAutoSave());
    }
    private IEnumerator DontAutoSave()
    {
        justLoaded = true;
        yield return new WaitForSeconds(5f); // 5초 후 자동 저장 허용
        justLoaded = false;
    }
}
