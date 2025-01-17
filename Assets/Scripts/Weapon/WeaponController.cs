using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;


public class WeaponController : MonoBehaviour
{
    //빨아들이는 범위 (콜라이더로 구현)
    public GameObject AbsorbRange;

    //빨아들이고 있는 상태를 설정
    private bool isAbsorbing = false;

    //3가지 원소를 빨아들이고 있는 상태 설정
    private bool isRockActive = false;
    private bool isGrassActive = false;
    private bool isWaterActive = false;

    public GameObject RockEffect;
    public GameObject WaterEffect;
    public GameObject GrassEffect;

    //게이지 현재값
    public float RockGauge = 0f;
    public float GrassGauge = 0f;
    public float WaterGauge = 0f;

    //게이지 최대값
    public float MaxGauge = 100f;
    //게이지가 차는 속도
    public float FillSpeed = 1f;

    //총을 놓을 기준점
    public Transform GunPivot;
    public Transform Gun;

    //총의 모드
    public string WeaponMode;

    //총 관련 설정
    public GameObject bulletPrefab;
    public GameObject bombPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireCooldown = 0.5f;
    public float BombFireCooldown = 2f;
    private bool canShoot = true;
    public Vector3 mouseWorldPosition;

    //발판 관련 설정
    public GameObject platformPrefab; // 발판 프리팹
    public float platformSpeed = 5f; // 발판 이동 속도
    public float maxPlatformDistance = 2f; // 발판이 이동할 최대 거리
    public int maxPlatforms = 2;
    private List<GameObject> activePlatforms = new List<GameObject>();

    //힐링 포션 변수
    public GameObject HealPrefab;
    private bool isGeneratingHealPotion;

    //물총 변수
    public GameObject WaterPrefab;

    public RopeActive rope;


    public static WeaponController Instance { get; private set; }

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

    private void Start()
    {
        //빨아들이는 범위를 꺼둠
        AbsorbRange.SetActive(false);
        RockEffect.SetActive(false);
        WaterEffect.SetActive(false);
        GrassEffect.SetActive(false);
    }

    private void Update()
    {

        //마우스의 현재 위치를 탐색하는 변수 설정 (메인카메라 활용)
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        mouseWorldPosition = UnityEngine.Camera.main.ScreenToWorldPoint
        (new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, transform.position.z - GunPivot.position.z));

        

        // 총구의 회전방향 설정
        Vector2 direction = new Vector2(
             mouseWorldPosition.x - GunPivot.position.x,
             mouseWorldPosition.y - GunPivot.position.y
         );
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //총이 머리 위나 바닥 아래로 지나가면 총이 뒤집하는 조건
        bool shouldFlipGun = angle > 90 || angle < -90;

        //뒤집히는 로직 설정
        if (shouldFlipGun)
        {
            Gun.rotation = Quaternion.Euler(180, 0, -angle); // 총을 뒤집음
        }
        else
        {
            Gun.rotation = Quaternion.Euler(0, 0, angle); // 총을 정상적으로 회전
        }

    }

    //오른마우스 눌렀을때
    public void AbsorbClick()
    {
        isAbsorbing = true;
        AbsorbRange.SetActive(true);
    }

    //오른마우스 뗐을때
    public void AbsorbClickUp()
    {
        isAbsorbing = false;
        AbsorbRange.SetActive(false);

        RockEffect.SetActive(false);
        GrassEffect.SetActive(false);
        WaterEffect.SetActive(false);
    }

    public void OnAbsorbEffectTriggerStay(Collider2D other)
    {
        Debug.Log($"OnTriggerStay2D 호출됨: {other.name}");

        if (!isAbsorbing)
        {
            Debug.Log("isAbsorbing이 false여서 리턴됨");
            return;
        }
        // 태그에 따라 효과 활성화
        switch (other.tag)
        {
            case "Rock":
                Debug.Log("인식");
                ActivateEffect(RockEffect);
                isRockActive = true;
                FillGauge();
                break;
            case "Grass":
                Debug.Log("인식");
                ActivateEffect(GrassEffect);
                isGrassActive = true;
                FillGauge();
                break;
            case "Water":
                Debug.Log("인식");
                ActivateEffect(WaterEffect);
                isWaterActive = true;
                FillGauge();
                break;
        }
    }

    public void OnAbsorbEffectTriggerExit(Collider2D other)
    {


        // 모든 효과를 비활성화
        DeactivateAllEffects();
    }

    private void ActivateEffect(GameObject effect)
    {

        // 다른 효과 비활성화
        DeactivateAllEffects();

        // 원하는 효과만 활성화
        effect.SetActive(true);
    }

    private void DeactivateAllEffects()
    {
        RockEffect.SetActive(false);
        GrassEffect.SetActive(false);
        WaterEffect.SetActive(false);
        isRockActive = false;
        isGrassActive = false;
        isWaterActive = false;
    }

    private void FillGauge()
    {
        if (isRockActive)
        {
            RockGauge += FillSpeed * Time.deltaTime;
            RockGauge = Mathf.Clamp(RockGauge, 0, MaxGauge);
            Debug.Log("돌 게이지: " + RockGauge);
        }

        if (isGrassActive)
        {
            GrassGauge += FillSpeed * Time.deltaTime;
            GrassGauge = Mathf.Clamp(GrassGauge, 0, MaxGauge);
            Debug.Log("풀 게이지: " + GrassGauge);
        }

        if (isWaterActive)
        {
            WaterGauge += FillSpeed * Time.deltaTime;
            WaterGauge = Mathf.Clamp(WaterGauge, 0, MaxGauge);
            Debug.Log("물 게이지: " + WaterGauge);
        }
    }

    //무기 고르는 (왼마우스 눌렀을 때 발동)
    public void WeaponSelect()
    {
        switch (WeaponMode)
        {
            case "water":
                StartCoroutine(WaterSpray()); //파도타기
                break;
            case "potion":
                StartCoroutine(HealPotion()); //회복포션
                break;
            case "bullet":
                StartCoroutine(RockBullet()); //돌총알
                break;
            case "rockBomb":
                StartCoroutine(RockBomb()); //돌폭탄
                break;
            case "treeVine":
                if (canShoot && GrassGauge > 0)
                {
                    GrassGauge -= 5f;
                    StartCoroutine(rope.RopeAction()); //나무덩쿨
                }
                break;
            case "platform":
                RockPlatform(); //발판
                break;
        }
    }

    public void WeaponLeft()
    {
        StartCoroutine(rope.RopeDetatch());
    }

    private IEnumerator RockBullet()
    {
        Debug.Log("발사");
        if (canShoot && RockGauge > 0 && WaterGauge > 0)
        {
            Debug.Log("발사2");
            WaterGauge -= 1f;
            RockGauge -= 1f;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // 총알의 Rigidbody2D에 속도 추가
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0; // 중력 영향 제거
                rb.linearVelocity = firePoint.right * bulletSpeed; // 발사 방향과 속도 설정
            }
            StartCoroutine(Cooldown(0.2f));
            yield return new WaitForSeconds(2f);
            Destroy(bullet);
        }
    }

    IEnumerator Cooldown(float CoolDown)
    {
        canShoot = false; // 발사 불가능 상태로 전환
        yield return new WaitForSeconds(CoolDown); // 쿨다운 시간 기다림
        canShoot = true; // 발사 가능 상태로 전환
    }

    private IEnumerator RockBomb()
    {
        Debug.Log("발사3");
        if (canShoot && RockGauge > 0)
        {
            Debug.Log("발사2");
            RockGauge -= 5f;
            GameObject Bomb = Instantiate(bombPrefab, firePoint.position, firePoint.rotation);

            // 총알의 Rigidbody2D에 속도 추가
            Rigidbody2D rb = Bomb.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * bulletSpeed; // 발사 방향과 속도 설정
            }
            StartCoroutine(Cooldown(3f));
            yield return null;
        }
    }


    public void RockPlatform()
    {
        Debug.Log("발사3");
        if (canShoot && RockGauge > 0 && GrassGauge > 0)
        {
            RockGauge -= 5f;
            GrassGauge -= 5f;

            if (activePlatforms.Count >= maxPlatforms)
            {
                // 활성화된 발판이 최대 갯수일 경우 첫 번째 발판 제거
                Destroy(activePlatforms[0]);
                activePlatforms.RemoveAt(0);
            }
            GameObject platform = Instantiate(platformPrefab, firePoint.position, firePoint.rotation);
            activePlatforms.Add(platform);
            Rigidbody2D rb = platform.GetComponent<Rigidbody2D>();

            rb.linearVelocity = firePoint.right * platformSpeed;
            platform.transform.rotation = Quaternion.identity;

            StartCoroutine(PlatformBehavior(platform, rb));
        }
    }


    IEnumerator PlatformBehavior(GameObject platform, Rigidbody2D rb)
    {
        float traveledDistance = 0f;
        Vector2 lastPosition = platform.transform.position;
        Vector3 initialScale = platform.transform.localScale;
        Vector3 targetScale = new Vector3(1f, 1f, 1f); // 최종 발판 크기

        while (traveledDistance < maxPlatformDistance)
        {
            // 발판 크기 점진적으로 변경
            platform.transform.localScale = Vector3.Lerp(initialScale, targetScale, traveledDistance / maxPlatformDistance);

            // 이동 거리 계산
            traveledDistance += Vector2.Distance(lastPosition, platform.transform.position);
            lastPosition = platform.transform.position;

            yield return null;
        }

        // 발판 고정
        rb.linearVelocity = Vector2.zero; // 속도 제거
        rb.GetComponent<Collider2D>().enabled = true; // 충돌 가능 활성화
    }

    IEnumerator HealPotion()
    {
       
        if (canShoot && !isGeneratingHealPotion && RockGauge > 0 && GrassGauge > 0)
        {
            isGeneratingHealPotion = true;
            //클릭쿨다운 문제를 해결해야할듯?
            WaterGauge -= 20f;
            GrassGauge -= 20f;

            yield return new WaitForSeconds(2f);
            GameObject Heal = Instantiate(HealPrefab, firePoint.position, firePoint.rotation);

            // 총알의 Rigidbody2D에 속도 추가
            Rigidbody2D rb = Heal.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * 5f; // 발사 방향과 속도 설정
            }
            StartCoroutine(Cooldown(5f));
            isGeneratingHealPotion = false;
        }
    }

    //물 무기 코드
    IEnumerator WaterSpray()
    {
        Debug.Log("발사");
        if (canShoot && WaterGauge > 0)
        {
            Debug.Log("발사2");
            WaterGauge -= 5f;

            Vector3 spawnPosition = firePoint.position + firePoint.right * 4f;
            GameObject WaterGun = Instantiate(WaterPrefab, spawnPosition, firePoint.rotation);

            // 총알의 Rigidbody2D에 속도 추가
            Rigidbody2D rb = WaterGun.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0; // 중력 영향 제거
                rb.linearVelocity = firePoint.right * 15f; // 발사 방향과 속도 설정
            }
            StartCoroutine(Cooldown(1f));
            yield return new WaitForSeconds(3f);
            Destroy(WaterGun);
        }
    }

}
