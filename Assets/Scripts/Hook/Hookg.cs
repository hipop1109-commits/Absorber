using System.Runtime.Serialization;
using UnityEngine;

public class Hookg : MonoBehaviour
{
    //로프 불러오기
    RopeActive grappling;
    //조인트 불러오기
    public DistanceJoint2D joint2D;

    private void Start()
    {
        grappling = GameObject.Find("Player").GetComponent<RopeActive>();
        joint2D = GetComponent<DistanceJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ring"))
        {
            Debug.Log("ㅇㅇ");
            joint2D.enabled = true;
            grappling.isAttach = true;
        }
    }
}
