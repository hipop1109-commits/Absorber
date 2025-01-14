using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;


public class WeaponController : MonoBehaviour
{
    //���Ƶ��̴� ���� (�ݶ��̴��� ����)
    public GameObject AbsorbRange;

    //���Ƶ��̰� �ִ� ���¸� ����
    private bool isAbsorbing = false;

    //3���� ���Ҹ� ���Ƶ��̰� �ִ� ���� ����
    private bool isRockActive = false;
    private bool isGrassActive = false;
    private bool isWaterActive = false;

    public GameObject RockEffect;
    public GameObject WaterEffect;
    public GameObject GrassEffect;

    //������ ���簪
    public float RockGauge = 0f;
    public float GrassGauge = 0f;
    public float WaterGauge = 0f;

    //������ �ִ밪
    public float MaxGauge = 100f;
    //�������� ���� �ӵ�
    public float FillSpeed = 1f;

    //���� ���� ������
    public Transform GunPivot;
    public Transform Gun;

    //���� ���
    public string WeaponMode;

    //�� ���� ����
    public GameObject bulletPrefab;
    public GameObject bombPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireCooldown = 0.5f;
    public float BombFireCooldown = 2f;
    private bool canShoot = true;
    public Vector3 mouseWorldPosition;

    //���� ���� ����
    public GameObject platformPrefab; // ���� ������
    public float platformSpeed = 5f; // ���� �̵� �ӵ�
    public float maxPlatformDistance = 1f; // ������ �̵��� �ִ� �Ÿ�
    public int maxPlatforms = 2;
    private List<GameObject> activePlatforms = new List<GameObject>();

    //���� ���� ����
    public GameObject HealPrefab;

    //���� ����
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
        //���Ƶ��̴� ������ ����
        AbsorbRange.SetActive(false);
        RockEffect.SetActive(false);
        WaterEffect.SetActive(false);
        GrassEffect.SetActive(false);
    }

    private void Update()
    {

        //���콺�� ���� ��ġ�� Ž���ϴ� ���� ���� (����ī�޶� Ȱ��)
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        mouseWorldPosition = UnityEngine.Camera.main.ScreenToWorldPoint
        (new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, transform.position.z - GunPivot.position.z));
        

        // �ѱ��� ȸ������ ����
        Vector2 direction = new Vector2(
             mouseWorldPosition.x - GunPivot.position.x,
             mouseWorldPosition.y - GunPivot.position.y
         );
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //���� �Ӹ� ���� �ٴ� �Ʒ��� �������� ���� �����ϴ� ����
        bool shouldFlipGun = angle > 90 || angle < -90;

        //�������� ���� ����
        if (shouldFlipGun)
        {
            Gun.rotation = Quaternion.Euler(180, 0, -angle); // ���� ������
        }
        else
        {
            Gun.rotation = Quaternion.Euler(0, 0, angle); // ���� ���������� ȸ��
        }

    }

    //�������콺 ��������
    public void AbsorbClick()
    {
        isAbsorbing = true;
        AbsorbRange.SetActive(true);
    }

    //�������콺 ������
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
        Debug.Log($"OnTriggerStay2D ȣ���: {other.name}");

        if (!isAbsorbing)
        {
            Debug.Log("isAbsorbing�� false���� ���ϵ�");
            return;
        }
        // �±׿� ���� ȿ�� Ȱ��ȭ
        switch (other.tag)
        {
            case "Rock":
                Debug.Log("�ν�");
                ActivateEffect(RockEffect);
                isRockActive = true;
                FillGauge();
                break;
            case "Grass":
                Debug.Log("�ν�");
                ActivateEffect(GrassEffect);
                isGrassActive = true;
                FillGauge();
                break;
            case "Water":
                Debug.Log("�ν�");
                ActivateEffect(WaterEffect);
                isWaterActive = true;
                FillGauge();
                break;
        }
    }

    public void OnAbsorbEffectTriggerExit(Collider2D other)
    {
       

        // ��� ȿ���� ��Ȱ��ȭ
        DeactivateAllEffects();
    }

    private void ActivateEffect(GameObject effect)
    {
        
        // �ٸ� ȿ�� ��Ȱ��ȭ
        DeactivateAllEffects();

        // ���ϴ� ȿ���� Ȱ��ȭ
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
            Debug.Log("�� ������: " + RockGauge);
        }

        if (isGrassActive)
        {
            GrassGauge += FillSpeed * Time.deltaTime;
            GrassGauge = Mathf.Clamp(GrassGauge, 0, MaxGauge);
            Debug.Log("Ǯ ������: " + GrassGauge);
        }

        if (isWaterActive)
        {
            WaterGauge += FillSpeed * Time.deltaTime;
            WaterGauge = Mathf.Clamp(WaterGauge, 0, MaxGauge);
            Debug.Log("�� ������: " + WaterGauge);
        }
    }

    //���� ������ (�޸��콺 ������ �� �ߵ�)
    public void WeaponSelect()
    {
        switch (WeaponMode)
        {
            case "�ĵ�Ÿ��":
                StartCoroutine(WaterSpray()); //�ĵ�Ÿ��
                break;
            case "���� ����":
                StartCoroutine(HealPotion()); //ȸ������
                break;
            case "���� �Ѿ�":
                StartCoroutine(RockBullet()); //���Ѿ�
                break;
            case "���� ��ź":
                StartCoroutine(RockBomb()); //����ź
                break;
            case 5:
                rope.RopeAction(); //��������
                break;
            case "���� ����":
                Debug.Log("�߻�4");
                RockPlatform(); //����
                break;
        }
    }

    private IEnumerator RockBullet()
    {
        Debug.Log("�߻�");
        if (canShoot && RockGauge > 0 && WaterGauge > 0)
        {
            Debug.Log("�߻�2");
            WaterGauge -= 1f;
            RockGauge -= 1f;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // �Ѿ��� Rigidbody2D�� �ӵ� �߰�
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0; // �߷� ���� ����
                rb.linearVelocity = firePoint.right * bulletSpeed; // �߻� ����� �ӵ� ����
            }
            StartCoroutine(Cooldown(0.2f));
            yield return new WaitForSeconds(2f);
            Destroy(bullet);
        }
    }

    IEnumerator Cooldown(float CoolDown)
    {
        canShoot = false; // �߻� �Ұ��� ���·� ��ȯ
        yield return new WaitForSeconds(CoolDown); // ��ٿ� �ð� ��ٸ�
        canShoot = true; // �߻� ���� ���·� ��ȯ
    }

    private IEnumerator RockBomb()
    {
        Debug.Log("�߻�3");
        if (canShoot && RockGauge > 0)
        {
            Debug.Log("�߻�2");
            RockGauge -= 5f;
            GameObject Bomb = Instantiate(bombPrefab, firePoint.position, firePoint.rotation);

            // �Ѿ��� Rigidbody2D�� �ӵ� �߰�
            Rigidbody2D rb = Bomb.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * bulletSpeed; // �߻� ����� �ӵ� ����
            }
            StartCoroutine(Cooldown(3f));
            yield return new WaitForSeconds(2f);
            Destroy(Bomb);
        }
    }

    
    public void RockPlatform()
    {
        Debug.Log("�߻�3");
        if (canShoot && RockGauge > 0 && GrassGauge > 0)
        {
            RockGauge -= 5f;
            GrassGauge -= 5f;

            if (activePlatforms.Count >= maxPlatforms)
            {
                // Ȱ��ȭ�� ������ �ִ� ������ ��� ù ��° ���� ����
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
        Vector3 targetScale = new Vector3(4f, 1f, 1f); // ���� ���� ũ��

        while (traveledDistance < maxPlatformDistance)
        {
            // ���� ũ�� ���������� ����
            platform.transform.localScale = Vector3.Lerp(initialScale, targetScale, traveledDistance / maxPlatformDistance);

            // �̵� �Ÿ� ���
            traveledDistance += Vector2.Distance(lastPosition, platform.transform.position);
            lastPosition = platform.transform.position;

            yield return null;
        }

        // ���� ����
        rb.linearVelocity = Vector2.zero; // �ӵ� ����
        rb.GetComponent<Collider2D>().enabled = true; // �浹 ���� Ȱ��ȭ
    }

    IEnumerator HealPotion()
    {
        if (canShoot && RockGauge > 0 && GrassGauge > 0)
        {
            //Ŭ����ٿ� ������ �ذ��ؾ��ҵ�?
            WaterGauge -= 20f;
            GrassGauge -= 20f;

            yield return new WaitForSeconds(2f);
            GameObject Bomb = Instantiate(HealPrefab, firePoint.position, firePoint.rotation);

            // �Ѿ��� Rigidbody2D�� �ӵ� �߰�
            Rigidbody2D rb = Bomb.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * 5f; // �߻� ����� �ӵ� ����
            }
            StartCoroutine(Cooldown(5f));
        }
    }
    
    //�� ���� �ڵ�
    IEnumerator WaterSpray()
    {
        Debug.Log("�߻�");
        if (canShoot && WaterGauge > 0)
        {
            Debug.Log("�߻�2");
            WaterGauge -= 5f;
            
            GameObject WaterGun = Instantiate(WaterPrefab, firePoint.position, firePoint.rotation);

            // �Ѿ��� Rigidbody2D�� �ӵ� �߰�
            Rigidbody2D rb = WaterGun.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0; // �߷� ���� ����
                rb.linearVelocity = firePoint.right * 15f; // �߻� ����� �ӵ� ����
            }
            StartCoroutine(Cooldown(1f));
            yield return new WaitForSeconds(3f);
            Destroy(WaterGun);
        }
    }

}
