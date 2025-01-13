using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    public int WeaponMode;

    //총 관련 설정
    public GameObject bulletPrefab;
    public GameObject bombPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireCooldown = 0.5f;
    public float BombFireCooldown = 2f;
    private bool canShoot = true;

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
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = UnityEngine.Camera.main.ScreenToWorldPoint
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

        //오른버튼을 클릭했을 때 빨아들이는 기능을 시작
        if (Input.GetMouseButton(1))
       {
            isAbsorbing = true;
            AbsorbRange.SetActive(true);
        }
        //오른 마우스를 떼면 흡수 중단
        if (Input.GetMouseButtonUp(1))
        {
            isAbsorbing = false;
            AbsorbRange.SetActive(false);

            RockEffect.SetActive(false);
            GrassEffect.SetActive(false);
            WaterEffect.SetActive(false);
        }
        //왼 마우스를 누르면 무기 선택
        if (Input.GetMouseButton(0))
        {
            WeaponSelect();
        }
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


    private void WeaponSelect()
    {
        switch (WeaponMode)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                StartCoroutine(RockBullet());
                break;
            case 4:
                StartCoroutine(RockBomb());
                break;
            case 5:
                break;
            case 6:
                break;
        }
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
            StartCoroutine(ShootCooldown());
            yield return new WaitForSeconds(2f);
            Destroy(bullet);
        }
    }

    IEnumerator ShootCooldown()
    {
        canShoot = false; // 발사 불가능 상태로 전환
        yield return new WaitForSeconds(fireCooldown); // 쿨다운 시간 기다림
        canShoot = true; // 발사 가능 상태로 전환
    }

    private IEnumerator RockBomb()
    {
        Debug.Log("발사");
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
            StartCoroutine(BombCooldown());
            yield return new WaitForSeconds(2f);
            Destroy(Bomb);
        }
    }

    IEnumerator BombCooldown()
    {
        canShoot = false; // 발사 불가능 상태로 전환
        yield return new WaitForSeconds(BombFireCooldown); // 쿨다운 시간 기다림
        canShoot = true; // 발사 가능 상태로 전환
    }

    private void WaterSpray()
    {

    }

    private void GrassRope()
    {

    }

    private void RockPlatform()
    {

    }
    private void HealPotion()
    {

    }
}
