using System.Collections;
using UnityEngine;

public class ArmadiloPattern : MonoBehaviour
{
    private bool isInCloseRange = false;
    private bool isInFarRange = false;

    [SerializeField] private GameObject CloseRange;
    [SerializeField] private GameObject FarRange;

    private Animator ani;



    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("감지1");

            if (collision.IsTouching(CloseRange.GetComponent<Collider2D>()) && collision.IsTouching(FarRange.GetComponent<Collider2D>()) && !isInCloseRange)
            {
                Debug.Log("감지2: 가까운 거리");
                isInCloseRange = true;
                StartCoroutine(CloseAttack());
            }

            // FarRange의 영역 안에 플레이어가 있는지 확인
            if (collision.IsTouching(FarRange.GetComponent<Collider2D>()) && !isInFarRange)
            {
                Debug.Log("감지3: 먼 거리");
                isInFarRange = true;
                StartCoroutine(FarAttack());
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision == CloseRange.GetComponent<Collider2D>())
            {
                Debug.Log("감지2");
                isInCloseRange = false;
            }

            // FarRange의 콜라이더와 비교
            if (collision == FarRange.GetComponent<Collider2D>())
            {
                Debug.Log("감지3");
                isInFarRange = false;
            }
        }
    }

    private IEnumerator CloseAttack()
    {
        if (isInCloseRange)
        {
            Stomp();
            yield return new WaitForSeconds(5);
        }
    }

    private IEnumerator FarAttack()
    {
        int result = Random.Range(0, 10);
        if (result < 6)
        {
            Tackle();
        }
        else
        {
            SpineAttack();
        }
        yield return new WaitForSeconds(5);
    }

    private void Stomp()
    {
        ani.SetTrigger("Stomp");
    }

    private void Tackle()
    {
        ani.SetTrigger("Angry");
    }

    private void SpineAttack()
    {
        ani.SetTrigger("Spine");
    }
}
