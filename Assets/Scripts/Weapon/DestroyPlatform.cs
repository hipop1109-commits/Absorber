using System.Collections;
using UnityEngine;

public class DestroyPlatform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyPlat());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyPlat()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
