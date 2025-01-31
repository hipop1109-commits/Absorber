using System;
using System.Collections;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        savePath = Application.persistentDataPath + "/saveSlot.json";
    }
    // 게임 세이브
    public void SaveGame()
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        SaveData saveData = new SaveData
        {
            playerX = playerController.transform.position.x,
            playerY = playerController.transform.position.y,
            playerHP = playerController.player.PlayerHp,
            energyCore = playerController.player.EnergyCore,
            lastSavedTime = DateTime.Now.ToString("g"),
            currentScene = SceneManager.GetActiveScene().name
        };

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);  

        Debug.Log("Game Saved: " + savePath);

        menuDisplayer = FindObjectOfType<MenuDisplayer>();
    }
    // 게임로드
    public SaveData LoadGame()
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<SaveData>(json);
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
                SceneManager.LoadScene(saveData.currentScene);
                StartCoroutine(WaitAndLoadData(saveData));
            }
            else
            {
                ApplySaveData(saveData);
            }
        }
    }
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

        playerController.player.RoadPlayerHp(saveData.playerHP);
        playerController.player.RoadEnergyCore(saveData.energyCore);
        playerController.transform.position = new Vector3(saveData.playerX, saveData.playerY, 0);
        //player.EnergyCoreTextUpdate();
        LifeDisplayer.Instance.SetLives(player.PlayerHp, player.PlayerMaxHp);
    }
}
