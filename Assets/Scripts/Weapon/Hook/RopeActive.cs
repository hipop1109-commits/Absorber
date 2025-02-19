using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

public class RopeActive : MonoBehaviour
{
    //로프 변수
    public LineRenderer line;
    public Transform hook;
    public bool isHookActive;
    public bool isLineMax;
    public bool isAttach;
    public WeaponController weapon;
    Vector3 mouseDir;
   

    private void Start()
    {
        line.positionCount = 2;
        line.endWidth = line.startWidth = 1f;
        line.SetPosition(0, weapon.Gun.position);
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true;
        isAttach = false;
        hook.gameObject.SetActive(false);
    }

    private void Update()
    {
        line.SetPosition(0, weapon.Gun.position);
        line.SetPosition(1, hook.position);

        Vector3 ropeDirection = hook.position - weapon.Gun.position;

        if (isHookActive)
        {
            float angle = Mathf.Atan2(ropeDirection.y, ropeDirection.x) * Mathf.Rad2Deg;
            weapon.Gun.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public IEnumerator RopeAction()
    {
        while (!isHookActive)
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
            hook.Translate(mouseDir.normalized * Time.deltaTime * 80);
            if (Vector2.Distance(transform.position, hook.position) > 12)
            {
                isLineMax = true;
            }
            yield return null; // 다음 프레임까지 대기
        }
        while (isLineMax && isHookActive && !isAttach)
        {
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * 80);
            if (Vector2.Distance(transform.position, hook.position) < 0.01f)
            {
                isHookActive = false;
                isLineMax = false;
                hook.gameObject.SetActive(false);
            }
            yield return null; // 다음 프레임까지 대기
        }
      
    }

    public IEnumerator RopeDetatch()
    {
        while (isAttach)
        {
            isAttach = false;
            isHookActive = false;
            isLineMax = false;
            hook.GetComponent<Hookg>().joint2D.enabled = false;
            hook.gameObject.SetActive(false);

        }
        yield return null; // 다음 프레임까지 대기
    }
}
