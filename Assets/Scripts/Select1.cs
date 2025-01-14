using UnityEngine;

public class Select1 : MonoBehaviour
{
    Animator select1;

    [SerializeField] private PlayerController playerController;
    void Start()
    {
        select1 = GetComponent<Animator>();
    }
    void Update()
    {
        if (playerController != null)
        {
            //select값에 따라 다른 애니메이션 트리거
            switch (playerController.GetSelect1())
            {
                case 1:
                    select1.SetTrigger("Water");
                    break;
                case 2:
                    select1.SetTrigger("Glass");
                    break;
                case 3:
                    select1.SetTrigger("Rock");
                    break;
            }
        }
    }
}
