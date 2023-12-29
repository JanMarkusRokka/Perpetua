using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class OverworldPlayer : MonoBehaviour
{
    public float MoveSpeed = 10;
    private Rigidbody _rb;

    private float horizontal;
    private float vertical;
    private bool interact;

    private List<Interactable> interactables = new List<Interactable>();

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        interact = Input.GetKeyDown(KeyCode.E);

        // Keeps count of chests within interacting area.
        if (interact && interactables.Count > 0)
        {
            if (interactables.Count > 0)
            {
                Interactable interactable = interactables.ElementAt(0);
                if (interactable.isActive) interactable.Interact();
                interactables.Remove(interactable);
                interactables.Add(interactable);
            }
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector3(horizontal, 0, vertical) * MoveSpeed + new Vector3(0, _rb.velocity.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable && interactable.isActive)
        {
            interactables.Add(interactable);
            interactable.OnEnterRange();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable)
        {
            interactables.Remove(interactable);
            interactable.OnExitRange();
        }
    }
}
