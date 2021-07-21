using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] PlayerInput _playerInput;
        [SerializeField] PlayerMovement _playerMovement;
        [SerializeField] PlayerCollision _playerCollision;
        [SerializeField] PlayerAction _playerAction;
        //[SerializeField] UIPlayerInventory _playerUI;

        public PlayerInput Input => _playerInput;
        public PlayerMovement Movement => _playerMovement;
        public PlayerCollision Collision => _playerCollision;
        public PlayerAction Action => _playerAction;
        public Rigidbody2D Rigidbody2D { get; private set; }
        public Animator Animator { get; private set; }
        public Collider2D Collider { get; private set; }

        public  List<string> ballList;

        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Animator = GetComponentInChildren<Animator>();
            Collider = GetComponent<Collider2D>();
        }

        public void UpdateUIList()
        {
            //_playerUI.UpdateList(ballList);
        }
    }
}