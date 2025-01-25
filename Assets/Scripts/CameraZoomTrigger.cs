using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraZoomTrigger : MonoBehaviour
{
    public float zoomOutFOV = 30f; // 트리거 안에서의 줌아웃 FOV
    public float zoomSpeed = 2f; // FOV 변경 속도
    public float originalFOV; // 카메라의 원래 FOV
    private bool isInTrigger = false; // 트리거 안에 있는지 확인

    
    void Update()
    {
        // 트리거 안에 있을 때와 아닐 때 FOV를 조정
        if (isInTrigger)
        {
            UnityEngine.Camera.main.fieldOfView = Mathf.Lerp(zoomOutFOV, originalFOV, Time.deltaTime * zoomSpeed);
        }
        else
        {
            UnityEngine.Camera.main.fieldOfView = Mathf.Lerp(originalFOV, zoomOutFOV, Time.deltaTime * zoomSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 트리거 안에 들어왔을 때
        if (other.CompareTag("Player"))
        {
            Debug.Log("DDDD");
            isInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 플레이어가 트리거를 나갔을 때
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
        }
    }
}