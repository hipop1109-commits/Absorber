using UnityEngine;

public class AbsorbEffect : MonoBehaviour
{
    private WeaponController weaponController;

    private void Start()
    {
        // 부모의 WeaponController 스크립트 참조
        weaponController = GetComponentInParent<WeaponController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // 부모의 WeaponController로 이벤트 전달
        if (weaponController != null)
        {
            weaponController.OnAbsorbEffectTriggerStay(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 부모의 WeaponController로 이벤트 전달
        if (weaponController != null)
        {
            weaponController.OnAbsorbEffectTriggerExit(other);
        }
    }
}
