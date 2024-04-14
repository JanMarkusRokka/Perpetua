using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent unityEvent;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<OverworldPlayer>())
        {
            unityEvent.Invoke();
        }
    }
}
