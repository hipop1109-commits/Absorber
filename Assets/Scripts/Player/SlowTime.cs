using UnityEngine;

public class SlowTime : MonoBehaviour
{
    [SerializeField] private float slowFactor = 0.5f;
    public float slowLength = 4f;
    public static SlowTime Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 게임의 시간 흐름을 느리게 만들기
    public void Slow()
    {
        // 게임 시간 배율을 slowFactor로 설정
        Time.timeScale = slowFactor;

        // 물리 계산이 시간 배율에 맞게 일관되도록 fixedDeltaTime을 조정
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    // 게임의 시간 흐름을 점차 원래 속도로 복원
    public void Back()
    {
        // 게임 시간 배율을 원래대로 설정
        Time.timeScale = 1f;

        // fixedDeltaTime도 기본 값으로 복원
        Time.fixedDeltaTime = 0.02f; // Unity의 기본 fixedDeltaTime
    }


}
