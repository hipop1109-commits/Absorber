using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 씬 배열
    public enum Scenes
    {
        //TutorialScene,
        Forest,
        BossForest,
        DesertNormalScene,
        DesertBossScene,
        SnowNormalScene,
        SnowBossScene
    }

    // 스테이지 추적 변수 
    private static int currentStage = 1;

    private static string targetScene;

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
            targetScene = nextScene.ToString(); // 목표 씬 저장
            SceneManager.LoadScene("LoadingScene"); // 로딩 씬 먼저 실행
            currentStage++;
        }
        else
        {
            Debug.Log("모든 스테이지 완료");
        }
    }

    public static void LoadSpecificScene(Scenes scene)
    {
        targetScene = scene.ToString();
        SceneManager.LoadScene("LoadingScene"); // 로딩 씬을 먼저 로드
    }

    // 로딩 씬에서 호출할 함수
    public static string GetTargetScene()
    {
        return targetScene;
    }
}
