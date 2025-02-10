using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cinemachine;

namespace FightingGame
{
    public class Environment : MonoBehaviour
    {
        [Inject, SerializeField]
        private CinemachineVirtualCamera _Camera;
        [Inject, SerializeField]
        private CinemachineConfiner2D    _Confiner;
        [SerializeField]
        private CompositeCollider2D      _Boundary;
        [SerializeField]
        private CreateSpot       _Player;
        [SerializeField]
        private List<CreateSpot> _Enemys;

        [Inject]
        public GlobalDataAccess DataAccess { get; }

        private void Awake()
        {
            _Confiner.m_BoundingShape2D = _Boundary;
        }

        public void Set(Player player) 
        {
            _Player.Set(player.Character);
            
            _Camera.Follow = player.Character.CameraFocus;

            _Confiner.InvalidateCache();
        }

        public void Set(Enemy enemy) 
        {
            if (_Enemys.Count <= 0) { return; }

            var guid  = enemy.Character.GUID;
            var index = guid % _Enemys.Count;

            _Enemys[index].Set(enemy.Character);
        }
    }
}
