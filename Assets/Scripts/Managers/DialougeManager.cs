using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialougeManager : MonoBehaviour
{
    public static DialougeManager instance; // 싱글톤
    private DialogueTrigger currentDialogue; // 현재 대화 트리거
    public Button nextButton; // 대화 진행 버튼

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 이벤트 등록
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(WaitForNextButton()); // 씬이 바뀌면 버튼 다시 찾기
    }

    private IEnumerator WaitForNextButton()
    {
        GameObject btnObj = null;

        // `NextButton`을 찾을 수 있을 때까지 대기
        while (btnObj == null)
        {
            btnObj = GameObject.FindWithTag("NextButton");
            yield return null; // 한 프레임 대기
        }

        nextButton = btnObj.GetComponent<Button>();

        if (nextButton != null && currentDialogue != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => currentDialogue.OnClickNext());
        }
    }

    public void SetCurrentDialogue(DialogueTrigger dialogue)
    {
        currentDialogue = dialogue;
    }

    // DialogueTrigger가 활성화될 때 버튼을 찾도록 함
    public void OnDialogueEnabled()
    {
        StartCoroutine(WaitForNextButton());
    }
}
