using UnityEngine;

public class WaterGun : MonoBehaviour
{
    public ObjectPool objectPool; // ObjectPool 참조
    private float lifeTime = 5f; // 총알의 지속 시간
    private float timer = 0f; // 타이머

    public void Initialize(ObjectPool pool, float lifeTime)
    {
        objectPool = pool;
        this.lifeTime = lifeTime; // 총알의 지속 시간 설정
    }

    private void OnEnable()
    {
        timer = 0f; // 타이머 초기화
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            // 타이머가 지속 시간을 넘으면 풀로 반환
            if (objectPool != null)
            {
                objectPool.ReturnObject(gameObject);
            }
        }
    }
}
