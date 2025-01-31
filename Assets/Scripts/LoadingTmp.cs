using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingTmp : MonoBehaviour
{
    public TMP_Text loadingText; // TMP 텍스트 참조
    private string baseText = "로딩중"; // 기본 텍스트
    private int dotCount = 0; // 마침표 개수
    private float interval = 0.3f; // 변경 간격

    private void Start()
    {
        StartCoroutine(AnimateText()); // 애니메이션 시작
    }

    IEnumerator AnimateText()
    {
        while (true)
        {
            dotCount = (dotCount + 1) % 4; // 0, 1, 2, 3 반복
            loadingText.text = baseText + new string('.', dotCount); // 마침표 추가
            yield return new WaitForSeconds(interval); // 대기 후 반복
        }
    }
}
