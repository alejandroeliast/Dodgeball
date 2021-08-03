using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldPlayer
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] PlayerInput _playerInput;
        [SerializeField] PlayerMovement _playerMovement;
        [SerializeField] PlayerCollision _playerCollision;
        [SerializeField] PlayerAction _playerAction;
        public UIPlayerInfo _playerUI;

        public int Index { get; set; }
        public int Health { get; set; }
        public PlayerInput Input => _playerInput;
        public PlayerMovement Movement => _playerMovement;
        public PlayerCollision Collision => _playerCollision;
        public PlayerAction Action => _playerAction;
        public Rigidbody2D Rigidbody2D { get; private set; }
        public Animator Animator { get; private set; }
        public Collider2D Collider { get; private set; } 

        void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponentInChildren<Animator>();
            Collider = GetComponent<Collider2D>();
        }
        void Start()
        {
            transform.parent.name = $"Player {Index + 1}";
            name = $"Player {Index + 1}";
        }
    }
}