using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        string nextScene = SceneController.GetTargetScene(); // 이동할 씬 가져오기
        if (string.IsNullOrEmpty(nextScene))
        {
            nextScene = "TutorialScene"; // 기본 씬 설정 (처음 시작할 때 대비)
        }
        float RandomTime = Random.Range(1f, 2f);
        yield return new WaitForSeconds(RandomTime); // 로딩 시간 연출 (페이드 효과 가능)

        SceneManager.LoadScene(nextScene); // 대상 씬 로드
    }
}
