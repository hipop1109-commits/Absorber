using UnityEngine;  public class Player {     public int PlayerHp { get; private set; } // 플레이어의 현재 HP
    public int PlayerMaxHp { get; private set; } // 플레이어의 최대 Hp

    // 캐릭터 속성
    public float MoveSpeed { get; private set; } // 캐릭터 이동 속도
    public float JumpSpeed { get; private set; } // 캐릭터 점프 속도
    public float DashSpeed { get; private set; } // 캐릭터 대쉬 속도
    public float DashCooldown { get; private set; } // 대쉬 쿨타임
    public float DashDuration { get; private set; } // 대쉬 지속 시간

    //에너지 코어
    public int EnergyCore { get; private set; }

    // Player 클래스 초기화
    public Player(int maxHp,/* float moveSpeed, float jumpSpeed,*/ float dashSpeed, float dashCooldown, float dashDuration)     {         PlayerMaxHp = maxHp; // 최대 HP 설정         PlayerHp = maxHp; // 현재 HP 초기화         /*        MoveSpeed = moveSpeed;                 JumpSpeed = jumpSpeed;*/         DashSpeed = dashSpeed;         DashCooldown = dashCooldown;         DashDuration = dashDuration;     }


    // 플레이어가 회복할 때 호출되는 메서드
    public void Heal(int amount)     {         PlayerHp = Mathf.Min(PlayerHp + amount, PlayerMaxHp); // HP가 최대 HP를 초과하지 않도록 설정         LifeDisplayer.Instance.SetLives(PlayerHp, PlayerMaxHp);     }

    //플레이어 데미지
    public void Damage(int damage)     {         PlayerHp -= damage;         PlayerHp = Mathf.Max(PlayerHp, 0); // 최소값을 0으로 고정         LifeDisplayer.Instance.SetLives(PlayerHp, PlayerMaxHp);     }      public bool IsAlive()     {         return PlayerHp > 0;     }

    //에너지코어 얻기
    public void GetEnergyCore(int amount)     {         EnergyCore += amount;         EnergyCoreTextUpdate();     }

    //에너지코어 사용
    public void SetEnergyCore(int amount)     {         EnergyCore -= amount;         EnergyCoreTextUpdate();     }

    //에너지코어 텍스트 업데이트
    public void EnergyCoreTextUpdate()     {
        // SkillDisplayer의 에너지 코어 텍스트 업데이트
        SkillDisplayer.Instance.energyCoreText.text = EnergyCore.ToString("D3");     }     public void RoadPlayerHp(int hp)
    {
        PlayerHp = Mathf.Clamp(hp, 0, PlayerMaxHp);
    }

    public void RoadEnergyCore(int energy)
    {
        EnergyCore = energy;
    }
 }  