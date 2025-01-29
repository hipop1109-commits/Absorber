using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 씬 배열
    public enum Scenes
    {
        //sTutorialScene,
        Forest,
        BossForest,
        DesertNormalScene,
        DesertBossScene,
        SnowNormalScene,
        SnowBossScene
    }

    // 스테이지 추적 변수 
    private static int currentStage = 0;

    // 닿으면 씬 전환
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }
    // 다음 씬 로드 
    public void LoadNextScene()
    {
        if (currentStage < System.Enum.GetValues(typeof(Scenes)).Length)
        {
            Scenes nextScene = (Scenes)currentStage;
            SceneManager.LoadScene(nextScene.ToString());
            currentStage++;
        }
        else
        {
            Debug.Log("모든 스테이지 완료");
        }
    }
}
