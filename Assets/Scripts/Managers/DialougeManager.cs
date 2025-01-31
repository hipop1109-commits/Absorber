using UnityEngine;
using UnityEngine.UI;

public class DialougeManager : MonoBehaviour
{
    public static DialougeManager instance; // 싱글톤
    private DialogueTrigger currentDialogue; // 현재 대화 트리거
    public Button nextButton; // 대화 진행 버튼

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SetCurrentDialogue(DialogueTrigger dialogue)
    {
        currentDialogue = dialogue;
        nextButton.onClick.RemoveAllListeners(); // 기존 연결 제거
        nextButton.onClick.AddListener(() => currentDialogue.OnClickNext()); // 새로운 트리거 연결
    }
}
