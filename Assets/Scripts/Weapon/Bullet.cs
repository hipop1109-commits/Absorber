using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ObjectPool objectPool; // ObjectPool 참조

    public void Initialize(ObjectPool pool)
    {
        objectPool = pool; // ObjectPool을 초기화
    }

    private void OnTriggerEnter2D(Collider2D other) { 
        // 적 또는 장애물과 충돌 처리
        if (other.gameObject.CompareTag("Enemy")) // 적 태그 확인
        {
           objectPool.ReturnObject(gameObject);
        }
        else if (other.gameObject.CompareTag("ground")) // 벽이나 다른 객체에 충돌
        {
           objectPool.ReturnObject(gameObject);
        }
    }
}
