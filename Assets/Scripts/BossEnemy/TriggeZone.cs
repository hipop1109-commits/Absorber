using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] private ArmadiloPattern boss; // 보스 몬스터 스크립트 참조

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("입구 감지: 보스 활성화");
            boss.ActivateMovingState(); // 보스의 상태를 Moving으로 변경
        }
    }
}
