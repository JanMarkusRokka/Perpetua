using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    // Notification
    public float LifeTime;
    private float deleteTime;

    private void Start()
    {
        deleteTime = Time.time + LifeTime;
    }

    private void Update()
    {
        if (Time.time > deleteTime) Destroy(gameObject);
    }
}
