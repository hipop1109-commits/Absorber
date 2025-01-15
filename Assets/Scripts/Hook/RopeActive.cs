using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeActive : MonoBehaviour
{

    //로프 변수
    public LineRenderer line;
    public Transform hook;
    public bool isHookActive = false;
    public bool isLineMax;
    public WeaponController weapon;
    public float elapsedTime = 0;
    public float ropeCoolTime = 5f;
    Vector3 mouseDir;


    private void Start()
    {
        line.positionCount = 2;
        line.endWidth = line.startWidth = 0.05f;
        line.SetPosition(0, weapon.Gun.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;

       
    }

    private void Update()
    {
        line.SetPosition(0, weapon.Gun.position);
        line.SetPosition(1, hook.position);
    }

    public IEnumerator RopeAction()
    {

        if (!isHookActive)
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

        while (!isLineMax)
        {
            hook.Translate(mouseDir.normalized * Time.deltaTime * 700);
            if (Vector2.Distance(transform.position, hook.position) > 10)
            {
                isLineMax = true;
            }
            yield return null; // 다음 프레임까지 대기
        }

        while (isLineMax)
        {
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * 15);
            if (Vector2.Distance(transform.position, hook.position) < 1f)
            {
                isHookActive = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
            }
            yield return null; // 다음 프레임까지 대기
        }
    }
}
