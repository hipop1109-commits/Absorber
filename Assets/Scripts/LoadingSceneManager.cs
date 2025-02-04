using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadScene());
        DisableSingletons();
    }

    private void OnDestroy()
    {
        EnableSingletons(); // 씬 떠날 때 싱글톤 다시 활성화
    }

    static public IEnumerator LoadScene()
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

    private void DisableSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(false);
    }

    private void EnableSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(true);
    }
}
