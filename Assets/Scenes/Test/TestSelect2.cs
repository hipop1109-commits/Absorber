using UnityEngine;

public class TestSelect2 : MonoBehaviour
{
    Animator select2;

    [SerializeField] private Test1 test;
    void Start()
    {
        select2 = GetComponent<Animator>();
    }
    void Update()
    {
        if (test != null)
        {
            //select값에 따라 다른 애니메이션 트리거
            switch (test.GetSelect2())
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
