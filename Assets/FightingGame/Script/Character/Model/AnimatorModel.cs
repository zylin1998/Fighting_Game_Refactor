using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FightingGame
{
    public class AnimatorModel : CharacterPropertyModel
    {
        public AnimatorModel(Character character) : base(character)
        {
            Animator = character.Animator;
        }

        public Animator Animator { get; }

        public void Play(string name, int layer = 0) 
        {
            Animator.Play(name, layer);
        }

        public float NormalizeTime(string name, int layer = 0) 
        {
            var state = Animator.GetCurrentAnimatorStateInfo(0);

            return state.IsName(name) ? state.normalizedTime : 0f;
        }
    }
}
