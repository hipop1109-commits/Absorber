using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(BombExplode());
    }

    private IEnumerator BombExplode()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

        Vector3 explosionPosition = transform.position + new Vector3(0, 3 , 0);
        GameObject explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
    }

}
