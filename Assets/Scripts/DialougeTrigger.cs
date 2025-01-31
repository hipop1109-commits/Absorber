using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speaker; // "a" 또는 "b"
        public string text;    // 대화 내용
    }

    public List<DialogueLine> dialogueLines; // 개별 트리거마다 다른 대화 내용
    public Image protagonistImage; // 주인공 이미지
    public Image npcImage;
    public TMP_Text leftTextBox;  // 주인공 대화 TMP 박스
    public TMP_Text rightTextBox; // 상대방 대화 TMP 박스
    public GameObject dialogueCanvas; // 대화 UI (캔버스)

    private int currentLineIndex = 0; // 현재 대화 인덱스 (각 트리거마다 개별 관리)
    private bool isDialogueActive = false; // 대화 진행 중인지 체크 (개별 관리)

    private static DialogueTrigger activeDialogue = null; // 현재 진행 중인 대화 (다른 트리거 방해 방지)
    private bool hasTriggered = false;
    void Start()
    {
        dialogueCanvas.SetActive(false); // 시작 시 대화 UI 비활성화
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDialogueActive && activeDialogue == null && !hasTriggered) // 현재 대화가 진행 중이 아닐 때만 실행
        {
            hasTriggered = true;
            DialougeManager.instance.SetCurrentDialogue(this);
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        isDialogueActive = true;
        activeDialogue = this; // 현재 대화를 활성화
        currentLineIndex = 0; // 이 트리거에서만 초기화
        dialogueCanvas.SetActive(true); // 대화 UI 활성화
        Time.timeScale = 0; // **시간 멈추기**
        ShowDialogueLine(); // 첫 대사 표시
    }

    public void OnClickNext()
    {
        if (currentLineIndex >= dialogueLines.Count - 1) // 마지막 대화인지 먼저 체크
        {
            EndDialogue();
            return;
        }

        currentLineIndex++; // 안전하게 증가
        Debug.Log($"현재 대화 인덱스: {currentLineIndex} / 총 대화 개수: {dialogueLines.Count}");
        ShowDialogueLine();
    }

    private void ShowDialogueLine()
    {
        var line = dialogueLines[currentLineIndex]; // 현재 대화 줄 가져오기

        // 대화 박스 초기화
        leftTextBox.text = "";
        rightTextBox.text = "";

        // 화자에 따라 텍스트 출력
        if (line.speaker == "a")
        {
            protagonistImage.color = Color.white; // 주인공 이미지 밝게
            npcImage.color = new Color(0.5f, 0.5f, 0.5f); // NPC 이미지 어둡게
            leftTextBox.text = line.text; // 주인공 대화 왼쪽 박스에 출력
        }
        else if (line.speaker == "b")
        {
            protagonistImage.color = new Color(0.5f, 0.5f, 0.5f); // 주인공 이미지 어둡게
            npcImage.color = Color.white; // NPC 이미지 밝게
            rightTextBox.text = line.text; // 상대방 대화 오른쪽 박스에 출력
        }
    }

    private void EndDialogue()
    {
        dialogueCanvas.SetActive(false); // 대화 UI 비활성화
        isDialogueActive = false; // 대화 종료 상태로 변경
        activeDialogue = null; // 현재 진행 중인 대화 해제
        Time.timeScale = 1; // **시간 다시 정상 속도로**
        currentLineIndex = 0;
        Debug.Log("대화가 끝났습니다.");
    }
}
