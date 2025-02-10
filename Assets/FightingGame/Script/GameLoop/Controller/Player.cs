using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace FightingGame
{
    public class Player
    {
        private bool _Enable;

        public MovementModel Movement  { get; private set; }

        public Property<bool>  Attack { get; private set; }
        public Property<bool>  IsJump { get; private set; }
        public Property<bool>  Dead   { get; private set; }

        public Character Character { get; private set; }

        public string Tag    => Character.tag;
        public int    GUID   => Character.GUID;
        public bool   IsDead => Dead.Value;

        public void Set(Character character) 
        {
            Character = character;

            Movement = Character.GetModel<MovementModel>();
            Attack   = Character.GetBoolean("Attack");
            IsJump   = Character.GetBoolean("Jump");
            Dead     = Character.GetBoolean("Dead");
        }

        public Character Release() 
        {
            var character = Character;

            Character?.Disable();

            Character = default;

            return character;
        }

        public void Enable()
        {
            _Enable = true;

            Character.Enable();

            Observable
                .EveryUpdate()
                .TakeWhile((l) => _Enable)
                .Subscribe(l => GetInput());
        }

        public void Disable() 
        {
            _Enable = false;

            Movement.Set(Vector2.zero);
        }

        private void GetInput()
        {
            Movement.Set(new(Input.GetAxisRaw("Horizontal"), 0f));

            Attack.Set(Input.GetKey(KeyCode.Q));
            
            IsJump.Set(Input.GetKeyDown(KeyCode.Space));
        }
    }
}
