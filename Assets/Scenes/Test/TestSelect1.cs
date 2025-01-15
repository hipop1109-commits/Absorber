using UnityEngine;

public class TestSelect1 : MonoBehaviour
{
    Animator select1;

    [SerializeField] private Test1 test;
    void Start()
    {
        select1 = GetComponent<Animator>();
    }
    void Update()
    {
        if (test != null)
        {
            //select값에 따라 다른 애니메이션 트리거
            switch (test.GetSelect1())
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
