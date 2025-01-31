using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    public GameObject prefab; // 풀링할 프리팹
    public int initialSize = 10; // 초기 풀 크기

    private Queue<GameObject> pool = new Queue<GameObject>();

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

        // 초기 풀 생성
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false); // 비활성화
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        // 사용 가능한 오브젝트가 있으면 반환
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true); // 활성화
            return obj;
        }

        // 없으면 새로 생성 (확장 가능)
        GameObject newObj = Instantiate(prefab, position, rotation);
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false); // 비활성화
        pool.Enqueue(obj); // 풀에 다시 추가
    }
}
