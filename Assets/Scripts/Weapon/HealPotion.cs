using System;
using UnityEngine;
using System.Collections.Generic;

public class HealPotion : MonoBehaviour
{ 
    public int HealAmount;
    [SerializeField] private PlayerController playerController;
    private bool isReadyToUse = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Invoke(nameof(EnableTrigger), 2f);
    }

    private void EnableTrigger()
    {
        isReadyToUse = true;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isReadyToUse) return;

        if (collision.CompareTag("Player"))
        {
            
            Destroy(gameObject);
        }
    }

}
