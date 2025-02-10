using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FightingGame
{
    public class JumpModel : CharacterPropertyModel
    {
        public JumpModel(Character character) : base(character)
        {
            JumpForce = character.GetFloat("JumpForce");

            IsJump      = character.GetBoolean("Jump");
            GroundCheck = character.GetBoolean("GroundCheck");
        }

        public Property<float> JumpForce  { get; }
        public Property<bool>  IsJump     { get; }
        public Property<bool> GroundCheck { get; }

        public bool Jumped => IsJump.Value;

        public void ForceUpdate() 
        {
            Character.Rigidbody.AddForce(new(0, JumpForce.Value), ForceMode2D.Impulse);

            GroundCheck.Set(false);
        }

        public override void Update()
        {
            if (!Jumped) { return; }

            Character.Rigidbody.AddForce(new(0, JumpForce.Value), ForceMode2D.Impulse);
        }
    }
}
