using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Serializable]
    public class SaveData
    {
        public float playerX;
        public float playerY;
        public int playerHP;
        public int energyCore;
    }

    private string savePath;
    private PlayerController playerController;
    private Player player;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/saveSlot.json";
    }
    
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerX = playerController.transform.position.x,
            playerY = playerController.transform.position.y,
            playerHP = playerController.player.PlayerHp,
            energyCore = playerController.player.EnergyCore
        };

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);  

        Debug.Log("Game Saved: " + savePath);

        MenuDisplayer saveSlot = FindObjectOfType<MenuDisplayer>();

        //// 슬롯 UI 업데이트 
        //foreach (var slot in saveSlot)
        //{
        //    slot.UpdateSlotUI();
        //}
    }
    // 
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
    public void OnSaveSlotClicked()
    {
        SaveData saveData = LoadGame();

        if(saveData != null)
        {
            playerController.player.RoadPlayerHp(saveData.playerHP);
            playerController.player.RoadEnergyCore(saveData.energyCore);

            playerController.transform.position = new Vector3(saveData.playerX, saveData.playerY, 0);
            player.EnergyCoreTextUpdate();
            LifeDisplayer.Instance.SetLives(player.PlayerHp, player.PlayerMaxHp);
        }
    }
}
