using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void OnClickResentGameStart()
    {
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
}
