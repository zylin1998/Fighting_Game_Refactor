using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;

namespace FightingGame
{
    public class MovementModel : CharacterPropertyModel
    {
        public MovementModel(Character character) : base(character)
        {
            MoveX     = character.GetFloat("MoveX");
            MoveY     = character.GetFloat("MoveY");
            MoveSpeed = character.GetFloat("MoveSpeed");

            GroundCheck = character.GetBoolean("GroundCheck");

            _Gravity = character.Rigidbody.gravityScale;
        }

        private float _Gravity;

        public float Gravity 
        {
            get => Character.Rigidbody.gravityScale; 
            
            set => Character.Rigidbody.gravityScale = value;
        }

        public bool Moving   => Direct != Vector2.zero;
        public bool IsGround { get => GroundCheck.Value; set => GroundCheck.Set(value); }
        public bool FreezeY 
        {
            get => Gravity == 0f; 
            
            set 
            {
                if (value == FreezeY) { return; }

                if ( value) { Gravity = 0f;       }
                if (!value) { Gravity = _Gravity; }
            }
        }

        public Vector2 Direct => new(MoveX.Value, MoveY.Value);

        public Property<float>  MoveX     { get; }
        public Property<float>  MoveY     { get; }
        public Property<float>  MoveSpeed { get; }

        public Property<bool> GroundCheck { get; }

        public void Set(float x, float y)
        {
            MoveX.Set(x.Clamp(-1, 1));
            MoveY.Set(y.Clamp(-1, 1));
        }

        public void Set(Vector2 direct) 
        {
            MoveX.Set(direct.x.Clamp(-1, 1));
            MoveY.Set(direct.y.Clamp(-1, 1));
        }

        public void Check()
        {
            var position   = Character.Position;
            var groundMask = Character.GroundMask;
            var velocity   = Character.Rigidbody.velocity;
            var collider   = Physics2D.OverlapCircle(position, 0.01f, groundMask);
            
            IsGround = collider && Mathf.Abs(velocity.y) <= 0.1f;
        }

        public void Move() 
        {
            Move(MoveSpeed.Value, Direct);
        }
        
        public void Move(float moveSpeed)
        {
            Move(moveSpeed, Direct);
        }

        public void Move(Vector2 direct)
        {
            Move(MoveSpeed.Value, direct);
        }

        public void Move(float moveSpeed, Vector2 direct)
        {
            var magnitude = direct.magnitude;

            var speedX = magnitude >= 0.1f ? direct / magnitude * moveSpeed : Vector2.zero;
            var speed = new Vector2(speedX.x, Character.Rigidbody.velocity.y);

            Character.Rigidbody.velocity = speed;
        }

        public void Flip() 
        {
            var direct = MoveX.Value;

            if (direct < 0 && Character.transform.localScale.x < 0) { return; }
            if (direct > 0 && Character.transform.localScale.x > 0) { return; }
            if (direct == 0) { return; }

            var scale = Character.transform.localScale;
            var side = direct > 0 ? 1f : -1f;

            Character.transform.localScale = new Vector3(Mathf.Abs(scale.x) * side, scale.y, scale.z);
        }

        public void Stop() 
        {
            Character.Rigidbody.velocity = Vector2.zero;
        }

        public void Push(Vector2 force) 
        {
            Character.Rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        public void Update(Vector2 direct) 
        {
            Set(direct);

            Update();
        }

        public override void Update()
        {
            Move();

            Flip();
        }
    }
}
