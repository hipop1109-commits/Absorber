using UnityEngine;

public class Select2 : MonoBehaviour
{
    Animator select2;

    [SerializeField] private PlayerController playerController;
    void Start()
    {
        select2 = GetComponent<Animator>();
    }
    void Update()
    {
        if (playerController != null)
        {
            //select값에 따라 다른 애니메이션 트리거
            switch (playerController.GetSelect2())
            {
                case 1:
                    select2.SetTrigger("Water");
                    break;
                case 2:
                    select2.SetTrigger("Glass");
                    break;
                case 3:
                    select2.SetTrigger("Rock");
                    break;
            }
        }
    }
}
