using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dialouge : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speaker; // "Protagonist" 또는 "NPC"
        public string text;    // 대화 내용
    }

    public List<DialogueLine> dialogueLines; // 대화 데이터

    public Image protagonistImage; // 주인공 이미지
    public Image npcImage;
    public TMP_Text leftTextBox;  // 주인공 대화 TMP 박스
    public TMP_Text rightTextBox; // 상대방 대화 TMP 박스

    private int currentLineIndex = 0; // 현재 대화 인덱스

    void Start()
    {
        ShowDialogueLine(); // 첫 대화 표시
        DisableSingletons();
    }

    private void OnDestroy()
    {
        EnableSingletons(); // 씬 떠날 때 싱글톤 다시 활성화
    }

    public void OnClickNext()
    {
        currentLineIndex++; // 다음 대화로 이동
        if (currentLineIndex < dialogueLines.Count)
        {
            ShowDialogueLine();
            AudioManager.Instance.ButtonSound();
        }
        else
        {
            EndDialogue(); // 대화 종료 처리
        }
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
        // 대화 종료 처리 (예: 텍스트 박스 초기화)
        leftTextBox.text = "";
        rightTextBox.text = "";
        Debug.Log("대화가 끝났습니다.");
        SceneController.LoadNextScene();
    }

    private void DisableSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(false);
        if (UIManager.Instance != null) UIManager.Instance.gameObject.SetActive(false);
    }

    private void EnableSingletons()
    {
        if (PlayerController.instance != null) PlayerController.instance.gameObject.SetActive(true);
        if (UIManager.Instance != null) UIManager.Instance.gameObject.SetActive(true);
    }
}
