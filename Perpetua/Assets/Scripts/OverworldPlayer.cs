using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OverworldPlayer : MonoBehaviour
{
    public float MoveSpeed = 10;
    public InventoryData inventory;
    private Rigidbody _rb;

    private float horizontal;
    private float vertical;
    private bool interact;

    private List<Chest> chests = new List<Chest>();

    public void Awake()
    {
        Events.OnItemReceived += OnItemReceived;
    }

    public void OnDestroy()
    {
        Events.OnItemReceived -= OnItemReceived;
    }

    public void OnItemReceived(ItemData item)
    {
        inventory.items.Add(item);
    }

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
        if (interact && chests.Count > 0)
        {
            Chest chest = chests.ElementAt(0);
            chest.GetItems();
            chest.StopIndicating();
            chests.Remove(chest);
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector3(horizontal, 0, vertical) * MoveSpeed + new Vector3(0, _rb.velocity.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Chest chest = other.GetComponent<Chest>();
        if (chest && !chest.isOpened)
        {
            chests.Add(chest);
            chest.StartIndicating();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Chest chest = other.GetComponent<Chest>();
        if (chest)
        {
            chests.Remove(chest);
            chest.StopIndicating();
        }
    }
}
