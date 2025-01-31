using UnityEngine;

public class WaterGun : MonoBehaviour
{
    private float lifeTime = 5f; // 총알의 지속 시간
    private float timer = 0f; // 타이머

    private void OnEnable() // 총알이 활성화될 때 타이머 초기화
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            gameObject.SetActive(false); // 3초 후 비활성화
        }
    }
}
