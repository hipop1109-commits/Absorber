using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    private MenuDisplayer mDisplayer;
    private GameObject menuPanel;
    [SerializeField] private GameObject noticePanel;
    private void Start()
    {
        mDisplayer = FindFirstObjectByType<MenuDisplayer>();
        menuPanel = mDisplayer.transform.Find("MenuPanel")?.gameObject;
        DisableSingletons();
    }

    private void OnDestroy()
    {
        EnableSingletons(); // æ¿ ∂∞≥Ø ∂ß ΩÃ±€≈Ê ¥ŸΩ√ »∞º∫»≠
    }


    public void OnClickResentGameStart()
    {
        SaveManager.SaveData saveData = SaveManager.Instance.LoadGame();
        if (saveData != null)
        {
            SaveManager.Instance.LoadGameData(saveData);
        }
        else
        {
            noticePanel.SetActive(true);
        }
    }
    public void OnClickGameStart()
    {
        SceneController.LoadNextScene();
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickSetting()
    {
        menuPanel.gameObject.SetActive(true);
    }
    private void DisableSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(false);
        if (SkillUIManager.Instance != null) SkillUIManager.Instance.gameObject.SetActive(false);
        if (UIManager.Instance != null) UIManager.Instance.gameObject.SetActive(false);
    }

    private void EnableSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(true);
        if (SkillUIManager.Instance != null) SkillUIManager.Instance.gameObject.SetActive(true);
        if (UIManager.Instance != null) UIManager.Instance.gameObject.SetActive(true);
    }
}
