using UnityEngine;
using UnityEngine.UI;

public class BossLifeDisplayer : MonoBehaviour
{
    // 보스 체력 바
    [SerializeField] private Image bossLifeBar;
    public BaseEnemy bossEnemy;
    // 보스 체력 바 맥스
    private int bossMaxHp; 

    private void Start()
    {
        //bossEnemy = GetComponent<BaseEnemy>();

        if(bossEnemy != null)
        {
            // 보스 체력 바 초기화
            bossMaxHp = bossEnemy.GetHp();
            bossLifeBar.fillAmount = bossEnemy.GetHp();
            // 보스 체력 변경이벤트 연결
            bossEnemy.BossHealthChaged += UpdateBossLife; 
        }
        //gameObject.SetActive(false);
    }
    // 트리거에 보스 체력 이벤트 연결 
    public void SetBossBar(BaseEnemy bEnemy)
    {
        bossEnemy = bEnemy;
        if (bossEnemy != null)
        {
            bossMaxHp = bossEnemy.GetHp();
            bossLifeBar.fillAmount = 1f;
            bossEnemy.BossHealthChaged += UpdateBossLife;
            bossEnemy.BossHealthChaged -= UpdateBossLife;
        }
    }
    // 보스 체력 바 업데이트
    public void UpdateBossLife(int bossCurrentLife)
    {
        bossLifeBar.fillAmount =(float)bossCurrentLife /bossMaxHp;
    }
    // 보스 체력 바 활성화
    public void ShowBossLifeBar()
    {
        gameObject.SetActive(true);
        Debug.Log("보스 체력 바 액티브");
    }
    // 보스 체력 바 비활성화
    public void HideBossLifeBar()
    {
        gameObject.SetActive(false);
    }
    // 이벤트 해제
    private void OnDestroy()
    {
        if(bossEnemy != null)
            bossEnemy.BossHealthChaged -= UpdateBossLife;
    }
}
