using UnityEngine;

public class RopeActive : MonoBehaviour
{

    //로프 변수
    public LineRenderer line;
    public Transform hook;
    public bool isHookActive = false;
    public bool isLineMax;
    public WeaponController weapon;

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

    public void RopeAction()
    {
        if (!isHookActive)
        {
            hook.position = weapon.firePoint.position;
            isHookActive = true;
            hook.gameObject.SetActive(true);
        }

        if (isHookActive && !isLineMax)
        {
            hook.Translate(weapon.mouseWorldPosition.normalized * Time.deltaTime * 50);

            if (Vector2.Distance(transform.position, hook.position) > 50)
            {
                isLineMax = true;
            }
        }
        else if (isHookActive && isLineMax)
        {
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * 15);
            if (Vector2.Distance(transform.position, hook.position) < 0.1f)
            {
                isHookActive = false;
                isLineMax = false;
            }
        }
    }
}
