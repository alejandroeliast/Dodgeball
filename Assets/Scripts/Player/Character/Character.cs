using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    


    public UIPlayerInfo _playerUI;

    public int Index { get; set; }
    public int Health { get; set; }
    public Controller Controller { get; private set; }
    public CharacterMovement Movement { get; private set; }
    public CharacterCollision Collision { get; private set; }
    public CharacterAction Action { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Animator Animator { get; private set; }
    public Collider2D Collider { get; private set; }

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
        Collider = GetComponent<Collider2D>();

        Movement = GetComponent<CharacterMovement>();
        Movement.Initialize(this);

        Collision = GetComponent<CharacterCollision>();
        Collision.Initialize(this);

        Action = GetComponent<CharacterAction>();
        Action.Initialize(this);
    }
    void Start()
    {
        name = $"Character {Controller.Index}";
    }
    internal void SetController(Controller controller)
    {
        Controller = controller;
    }
}
