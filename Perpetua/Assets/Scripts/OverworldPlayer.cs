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
    private Animator _anim;

    private float horizontal;
    private float vertical;
    private float horizontalRaw;
    private float verticalRaw;
    private bool interact;
    public GameObject PartyMemberPrefab;
    private List<Interactable> interactables = new List<Interactable>();

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        List<PartyCharacterData> partyMembers = PartyManager.Instance.party.PartyMembers;
        for (int i = 0; i < partyMembers.Count - 1; i++)
        {
            PartyCharacterData member = partyMembers[i];
            GameObject memberObject = Instantiate(PartyMemberPrefab, transform.position, transform.rotation);
            memberObject.GetComponent<OverworldPartyMember>().SetupCharacter(member, transform);
        }
        GetComponent<SpriteRenderer>().sprite = partyMembers[partyMembers.Count - 1].image;
        _anim = GetComponent<Animator>();
    }
    /*
    public void Awake()
    {
        Events.OnBattleTriggered += OnBattleTriggered;
    }

    public void OnDestroy()
    {
        Events.OnBattleTriggered -= OnBattleTriggered;
    }

    private void OnBattleTriggered(EnemyData enemyData)
    {
        enabled = false;
    }
    */
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        horizontalRaw = Input.GetAxisRaw("Horizontal");
        verticalRaw = Input.GetAxisRaw("Vertical");
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
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        bool moveBackwards = false;
        bool moveForwards = false;
        bool moveRight = false;
        bool moveLeft = false;
        if (verticalRaw > 0)
        {
            moveBackwards = true;
        }
        else if (verticalRaw < 0)
        {
            moveForwards = true;
        }

        if (horizontalRaw > 0)
        {
            moveRight = true;
        }
        else if (horizontalRaw < 0)
        {
            moveLeft = true;
        }

        _anim.SetBool("MoveBackwards", moveBackwards);
        _anim.SetBool("MoveForwards", moveForwards);
        _anim.SetBool("MoveRight", moveRight);
        _anim.SetBool("MoveLeft", moveLeft);
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
