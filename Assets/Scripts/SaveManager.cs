using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Serializable]
    public class SaveData
    {
        public int playerLife;
        public int weaponLevel;
        public int coreAmount;
        public Vector3 playerPosition;
        public string lastSavedTime;
    }

    // 자동저장
    public static void AutoSave(int slot, int life, int weaponLe, int coreAm, Vector3 positon)
    {
        SaveData data = new SaveData
        {
            playerLife = life,
            weaponLevel = weaponLe,
            coreAmount = coreAm,
            playerPosition = positon,
            lastSavedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        string json = JsonUtility.ToJson(data); // JSON 문자열로 변환
        PlayerPrefs.SetString("SaveSlot" + slot, json); // PlayerPrefs에 저장
        PlayerPrefs.Save(); // 저장 실행
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
        return null;
    }
    // 저장 슬롯 상태 확인 
    public static bool IsSlotEmpty(int slot)
    {
        return !PlayerPrefs.HasKey("SaveSlot" +  slot); 
    }
}
