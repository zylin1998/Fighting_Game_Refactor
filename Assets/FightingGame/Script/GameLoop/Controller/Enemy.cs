using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace FightingGame
{
    public class Enemy
    {
        public Enemy() 
        {

        }

        public Enemy(Player player, ItemFacadePool itemPool)
        {
            Set(player);

            ItemPool = itemPool;
        }

        private bool  _Enable;
        private bool  _Gathered = false;
        private float _AwaitRelease;

        public ItemFacadePool ItemPool { get; }

        public ItemFacade Item { get; private set; }

        public MovementModel Movement { get; private set; }
        public TrackingModel Tracking { get; private set; }

        public Property<int> Gather { get; private set; }

        public Property<bool>  Attack { get; private set; }
        public Property<bool>  Dead   { get; private set; }
        
        public Player    Player    { get; private set; }
        public Character Character { get; private set; }

        public bool IsDead     => Dead.Value;
        public bool CanRelease => _AwaitRelease <= 0f;
        
        public void Set(Player player) 
        {
            Player = player;
        }

        public void Set(Character character) 
        {
            Character = character;

            Gather = Character.GetInteger("Gather");
            Attack = Character.GetBoolean("Attack");
            Dead   = Character.GetBoolean("Dead");

            Movement = Character.GetModel<MovementModel>();
            Tracking = Character.GetModel<TrackingModel>();
            Tracking.SetTarget(Player.Character);

            _Gathered = false;
        }

        public Character Release() 
        {
            var character = Character;

            Character.Disable();

            Character = default;

            if (Item)
            {
                ItemPool.Despawn(Item);

                Item = default;
            }

            return character;
        }

        public void Enable() 
        {
            _Enable = true;

            _AwaitRelease = 2f;

            Character.Enable();

            Observable
                .EveryUpdate()
                .TakeWhile((l) => _Enable)
                .Subscribe((l) => GetState());
        }

        public void Disable() 
        {
            _Enable = false;
        }

        public void GetState() 
        {
            if (Dead.Value)
            {
                _AwaitRelease -= Time.deltaTime;

                return;
            }

            var distance = Tracking.Distance;
            var reached  = Tracking.Magnitude <= 0.8f;
            var tracking = Tracking.Tracking;

            var move     = !reached && tracking ? distance.x : 0;
            var attack   =  reached && tracking;
            
            Movement.Set(move, 0f);

            Attack.Set(attack);
        }

        public int Gathered()
        {
            if (_Gathered) { return 0; }

            _Gathered = true;

            Item = ItemPool.Spawn("Coin_Facade");

            Item.transform.position = Character.Position;

            Item.Enable();

            return Gather.Value;
        }
    }
}
