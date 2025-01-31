using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpBlock : MonoBehaviour
{
    public float riseHeight = 2f; // 상승할 높이
    public float riseSpeed = 2f;  // 상승 속도
    public bool returnToStart = false; // 원래 위치로 돌아올지 여부
    public float returnDelay = 2f; // 원래 위치로 돌아오기 전 대기 시간

    private Vector3 startPos; // 시작 위치 저장
    private bool isMoving = false; // 현재 움직이고 있는지 체크

    void Start()
    {
        startPos = transform.position; // 시작 위치 저장
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isMoving) // 플레이어가 밟았고, 아직 움직이고 있지 않을 때
        {
            StartCoroutine(MoveBlock(startPos + Vector3.up * riseHeight)); // 상승 코루틴 시작
        }
    }

    IEnumerator MoveBlock(Vector3 targetPos)
    {
        isMoving = true; // 이동 시작
        float elapsedTime = 0;
        Vector3 initialPos = transform.position;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(initialPos, targetPos, elapsedTime * riseSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // 정확한 위치 보정

        if (returnToStart)
        {
            yield return new WaitForSeconds(returnDelay); // 되돌아가기 전 대기
            StartCoroutine(MoveBlock(startPos)); // 원래 위치로 이동
        }
        else
        {
            isMoving = false; // 움직임 종료
        }
    }
}
