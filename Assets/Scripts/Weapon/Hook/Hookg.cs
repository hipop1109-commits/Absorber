using System.Runtime.Serialization;
using UnityEngine;

public class Hookg : MonoBehaviour
{
    public static Hookg Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        grappling = GameObject.Find("Player").GetComponent<RopeActive>();
        Debug.Log(grappling);
        joint2D = GetComponent<DistanceJoint2D>();
    }

    //로프 불러오기
    RopeActive grappling;
    //조인트 불러오기
    public DistanceJoint2D joint2D;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ring"))
        {
            joint2D.enabled = true;
            grappling.isAttach = true;
        }
    }
}
