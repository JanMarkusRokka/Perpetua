using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public struct Movement
{
    public bool left;
    public bool right;
    public bool back;
    public bool front;
}
public class VelocityAnimator : MonoBehaviour
{
    private Movement movement;
    private Animator _anim;
    Vector3 lastPos;
    public float Threshold = 0.02f;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = transform.position - lastPos;
        movement.right = false;
        movement.left = false;
        movement.front = false;
        movement.back = false;
        if (velocity.x > Threshold)
        {
            movement.right = true;
        }
        else if (velocity.x < -Threshold)
        {
            movement.left = true;
        }
        if (velocity.z > Threshold)
        {
            movement.back = true;
        }
        else if (velocity.z < -Threshold)
        {
            movement.front = true;
        }
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetBool("left", movement.left);
        _anim.SetBool("right", movement.right);
        _anim.SetBool("back", movement.back);
        _anim.SetBool("front", movement.front);
    }

}
