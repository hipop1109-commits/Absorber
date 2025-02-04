using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    private void Start()
    {
        DisableSingletons();
    }

    private void OnDestroy()
    {
        EnableSingletons(); // 씬 떠날 때 싱글톤 다시 활성화
    }
    public void OnClickResentGameStart()
    {
        SaveManager.SaveData saveData = SaveManager.Instance.LoadGame();
        SaveManager.Instance.LoadGameData(saveData);
        Debug.Log("저장된 게임 불러오기");
    }
    public void OnClickGameStart()
    {
        SceneController.LoadNextScene();
    }
    // 메뉴 패널 닫기
    public void OnClickQuit()
    {
        Application.Quit();
    }

    private void DisableSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(false);
        if (SkillUIManager.Instance != null) SkillUIManager.Instance.gameObject.SetActive(false);
    }

    private void EnableSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(true);
        if (SkillUIManager.Instance != null) SkillUIManager.Instance.gameObject.SetActive(true);
    }
}
