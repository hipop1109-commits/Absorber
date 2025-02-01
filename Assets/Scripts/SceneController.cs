using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 씬 배열
    public enum Scenes
    {
        TutorialScene,
        Forest,
        BossForest,
        DessertNormalScene,
        DessertBossScene,
        SnowNormalScene,
        SnowBossScene
    }

    private static Dictionary<Scenes, Vector2> spawnPositions = new Dictionary<Scenes, Vector2>()
    {
        { Scenes.TutorialScene, new Vector2(-41.5f, 3.3f) },
        { Scenes.Forest, new Vector2(-131, 5.39f) },
        { Scenes.BossForest, new Vector2(375, -6.1f) },
        { Scenes.DessertNormalScene, new Vector2(196.2f, -3.3f) },
        { Scenes.DessertBossScene, new Vector2(131.6f, -4f) },
        { Scenes.SnowNormalScene, new Vector2(-5, 2) },
        { Scenes.SnowBossScene, new Vector2(7, -4) }
    };

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

    public static Vector2 GetSpawnPosition(Scenes scene)
    {
        if (spawnPositions.ContainsKey(scene))
        {
            return spawnPositions[scene];
        }
        return Vector2.zero; // 기본값 (0,0)
    }

    // 로딩 씬에서 호출할 함수
    public static string GetTargetScene()
    {
        return targetScene;
    }
}
