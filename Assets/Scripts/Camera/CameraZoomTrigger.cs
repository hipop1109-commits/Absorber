using UnityEngine;

public class CameraZoomTrigger : MonoBehaviour
{
    public float zoomedOutFOV = 80f; // 줌아웃할 때 FOV
    public float normalFOV = 60f; // 기본 FOV
    public float zoomSpeed = 2f; // 줌 속도

    private bool isZoomed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 들어오면
        {
            isZoomed = true;
            StopAllCoroutines();
            StartCoroutine(ChangeFOV(zoomedOutFOV)); // 줌아웃
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 나가면
        {
            isZoomed = false;
            StopAllCoroutines();
            StartCoroutine(ChangeFOV(normalFOV)); // 원래대로
        }
    }

    System.Collections.IEnumerator ChangeFOV(float targetFOV)
    {
        float startFOV = UnityEngine.Camera.main.fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            UnityEngine.Camera.main.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime * zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UnityEngine.Camera.main.fieldOfView = targetFOV;
    }
}
