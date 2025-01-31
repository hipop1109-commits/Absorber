using UnityEngine;
using UnityEngine.InputSystem;

public class GunAim : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform firePoint; // 총구 위치
    public float maxDistance = 10f; // 최대 조준 거리

    private void Update()
    {
        DrawAimLine();
    }

    private void DrawAimLine()
    {
        
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = UnityEngine.Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, -UnityEngine.Camera.main.transform.position.z)
        );

   
        Vector2 direction = (mouseWorldPosition - firePoint.position).normalized;

       
        Vector2 targetPosition = (Vector2)firePoint.position + direction * maxDistance;

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, targetPosition);

        Debug.DrawLine(firePoint.position, targetPosition, Color.red);
    }
}
