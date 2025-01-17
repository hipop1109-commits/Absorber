using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplode : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(BombDestroy());
    }

    private IEnumerator BombDestroy()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }

}
