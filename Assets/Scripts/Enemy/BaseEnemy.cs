using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("공통 설정")]
    public bool isDie = false; // 적이 사망했는지 여부
    protected Rigidbody2D rb; // Rigidbody2D 참조
    [SerializeField] protected int hp = 20; // 적 체력
    public int damage = 10; // 적 공격력
    [SerializeField] protected GameObject itemPrefab; // 드랍할 아이템 프리팹
    public PlayerController playerController; // 플레이어 컨트롤러 참조
    public EnemyStateMachine stateMachine; // 상태 머신 참조
    private HashSet<Collider2D> processedAttacks = new HashSet<Collider2D>(); // 이미 처리된 Attack 오브젝트 추적

    public event System.Action<int> BossHealthChaged; // 보스 체력 바 변경 이벤트
    public int GetHp() => hp; // 보스 현재 체력 가져오기

    protected virtual void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        rb = GetComponent<Rigidbody2D>();
        // 상태 머신 초기화
        stateMachine = new EnemyStateMachine(this);

    }
    protected virtual void Start()
    {
        // 상태 머신의 초기 상태를 자식에서 설정
    }

    protected virtual void Update()
    {
        if (isDie) return;
        PerformMovement(); // 이동 로직
    }

    protected abstract void PerformMovement(); // 이동 로직(이동형/고정형에서 각각 구현)


    //적이 공격을 받으면
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDie) return; // 이미 죽은 적은 충돌 무시

        if (collision.gameObject.CompareTag("Attack") && !processedAttacks.Contains(collision))
        {
            processedAttacks.Add(collision); // 처리된 Attack 저장

            Attack attack = collision.GetComponent<Attack>();
            if (attack != null)
            {
                EnemyTakeDamage(attack.damage);
                StartCoroutine(DestroyAttack(attack.gameObject)); // attack.gameObject 전달            }
            }
        }
    }
    private IEnumerator DestroyAttack(GameObject attackObject)
    {
        yield return new WaitForSeconds(0.5f); 
        if (attackObject != null)
        {
            Destroy(attackObject); // attackObject 제거
        }
    }

    // 데미지를 입을때
    public void EnemyTakeDamage(int damage)
    {
        if (isDie) return;

        hp -= damage;
        BossHealthChaged?.Invoke(hp); // 보스 체력 바 이벤트 실행
        if (hp <= 0)
        {
            Die();
            AudioManager.Instance.PlaySound(AudioManager.AudioType.EnemyDie);
            Debug.Log("Die");
        }
        else
        {
            stateMachine.TransitionTo(stateMachine.hitState);
            AudioManager.Instance.PlaySound(AudioManager.AudioType.EnemyHurt);
            Debug.Log($"{gameObject.name} 체력: {hp}");
        }
    }

    /// <summary>
    /// 적의 Attack
    /// </summary>
    public void Attack(int damage)
    {
        //스킬 데미지(TakeDamage는 적에게 닿았을때 체력이 닳는 메서드기 때문에 Attack을 받았을때 조금 더 닳게 함)
        playerController.TakeDamage(damage + 10);
        Debug.Log("스킬 데미지");
    }

    /// <summary>
    /// 적이 죽는 메서드
    /// </summary>
    protected virtual void Die()
    {
        isDie = true; // 사망 상태 설정
        stateMachine.TransitionTo(stateMachine.dieState);
        Debug.Log(stateMachine.CurrentState);
        Debug.Log($"{gameObject.name} 사망");

        // 보스 체력 바 비활성화
        BossLifeDisplayer bossLifeDisplayer = GetComponent<BossLifeDisplayer>();
        if (bossLifeDisplayer != null)
        {
            bossLifeDisplayer.HideBossLifeBar();
        }

        // 일정 시간 후 적 제거
        Invoke("DestroyEnemy", 1f);
    }

    void DestroyEnemy()
    {
        //적 사라짐
        Destroy(gameObject);
        //아이템 드랍
        SpawnItem();
    }


    protected virtual void SpawnItem()
    {
        if (itemPrefab != null)
        {
            // 적 위치보다 약간 위에 아이템z 생성
            GameObject item = Instantiate(itemPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 위로 올라가는 힘을 줌
                rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

                // 중력을 해제하여 바닥으로 빠지는 문제 방지
                rb.gravityScale = 1f; // 중력을 줄여 자연스러운 움직임 유지
            }
        }
    }

}
