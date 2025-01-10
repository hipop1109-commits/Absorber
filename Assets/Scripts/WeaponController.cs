using UnityEngine;
using UnityEngine.TextCore.Text;


public class WeaponController : MonoBehaviour
{
    //빨아들이는 범위 (콜라이더로 구현)
    //public GameObject AbsorbRange;

    //빨아들이고 있는 상태를 설정
    private bool isAbsorbing = false;

    //3가지 원소를 빨아들이고 있는 상태 설정
    private bool isRockActive = false;
    private bool isGrassActive = false;
    private bool isWaterActive = false;

    //public GameObject RockEffect;
    //public GameObject WaterEffect;
    //public GameObject GrassEffect;

    //게이지가 차는 속도
    public float FillSpeed = 0.1f;

    //총을 놓을 기준점
    public Transform GunPivot;
    public Transform Gun;

    //총의 모드
    public int WeaponMode;
    
    private void Start()
    {
        //빨아들이는 범위를 꺼둠
        //AbsorbRange.SetActive(false);
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
        //if (Input.GetMouseButton(1))
       // {
        //    Absorb();
        //}

       // if(Input.GetMouseButton(0))
       // {
       //     WeaponSelect();
       // }
    }

    private void Absorb()
    {
        isAbsorbing = true;
        //AbsorbRange.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isAbsorbing) return;

        // 태그에 따라 효과 활성화
        switch (other.tag)
        {
            case "Rock":
                //ActivateEffect(RockEffect);
                isRockActive = true;
                FillGauge();
                break;
            case "Tree":
               //ActivateEffect(GrassEffect);
                isGrassActive = true;
                FillGauge();
                break;
            case "Water":
               // ActivateEffect(WaterEffect);
                isWaterActive = true;
                FillGauge();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // SuckEffect가 충돌한 Collider에서 빠져나갈 때 호출
        Debug.Log($"OnTriggerExit2D 호출: {other.name}");

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
        //RockEffect.SetActive(false);
        //GrassEffect.SetActive(false);
        //WaterEffect.SetActive(false);
        isRockActive = false;
        isGrassActive = false;
        isWaterActive = false;
    }

    private void FillGauge()
    {
        
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
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
    }

    private void RockBullet()
    {

    }

    private void RockBomb()
    {

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
