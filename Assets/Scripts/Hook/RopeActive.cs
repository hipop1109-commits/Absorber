using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeActive : MonoBehaviour
{

    //로프 변수
    public LineRenderer line;
    public Transform hook;
    public bool isHookActive;
    public bool isLineMax;
    public bool isAttach;
    public WeaponController weapon;
    public float elapsedTime = 0;
    public float ropeCoolTime = 5f;
    Vector3 mouseDir;
    public Rigidbody2D characterRigidbody;

    private void Start()
    {
        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        line.SetPosition(0, weapon.Gun.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isAttach = false;
       
    }

    private void Update()
    {
        line.SetPosition(0, weapon.Gun.position);
        line.SetPosition(1, hook.position);
    }

    public IEnumerator RopeAction()
    {
        characterRigidbody.bodyType = RigidbodyType2D.Kinematic;

        if (!isHookActive && !isLineMax)
        {
            hook.position = weapon.firePoint.position;
            Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 worldMousePosition = UnityEngine.Camera.main.ScreenToWorldPoint(
                new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Mathf.Abs(UnityEngine.Camera.main.transform.position.z))
                );
            mouseDir = (worldMousePosition - transform.position);
            mouseDir.z = 0;
            isHookActive = true;
            hook.gameObject.SetActive(true);
        }

        while (!isLineMax && isHookActive && !isAttach) 
        {
            hook.Translate(mouseDir.normalized * Time.deltaTime * 50);
            if (Vector2.Distance(transform.position, hook.position) > 10)
            {
                isLineMax = true;
            }
            
            yield return null; // 다음 프레임까지 대기

            
        }
        

        while (isLineMax && isHookActive && !isAttach)
        {
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * 50);
            if (Vector2.Distance(transform.position, hook.position) < 0.01f)
            {
                isHookActive = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
            }
            yield return null; // 다음 프레임까지 대기
        }

        while (isAttach)
        {

        }
        characterRigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.CompareTag("ground"))
        {
            characterRigidbody.bodyType = RigidbodyType2D.Dynamic;

            // 속도를 초기화해서 이상한 움직임 방지
            characterRigidbody.linearVelocity = Vector2.zero;
        }
    }
}
