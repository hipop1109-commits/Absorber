using System;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float cameraSpeed = 5.0f;

    public GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);
        this.transform.Translate(moveVector);
    }
}
