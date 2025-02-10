using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;
using Loyufei;

namespace FightingGame
{
    public class GameLoop
    {
        #region Field

        private int _GUID = 0;

        #endregion

        #region Injection

        [Inject]
        public EnvironmentPool  EnvironmentPool { get; }
        [Inject(Id = "Player")]
        public CharacterPool    PlayerPool      { get; }
        [Inject(Id = "Enemy")]
        public CharacterPool    EnemyPool       { get; }
        [Inject]
        public ItemFacadePool   ItemPool        { get; }
        [Inject]
        public GameReportModel  Report          { get; }
        
        #endregion

        #region Information

        public QuestInfo   Info        { get; private set; }
        public Environment Environment { get; private set; }
        public Player      Player      { get; private set; }

        public float Spawn { get; private set; }
        
        public Queue<Enemy>   StandBy { get; } = new();
        public Queue<string>  Await   { get; private set; }
        public HashSet<Enemy> OnScene { get; private set; } = new();

        public List<int> GUIDs { get; private set; }

        public GameResult GameResult
            => new(!Player.IsDead && !Report.GameOver, Report.GameTime, Report.TotalInjured, Report.TotalGather);

        private Loyufei.Random _Random = new();

        #endregion

        public GameLoop() 
        {
            Physics2D.IgnoreLayerCollision(7, 7);
            Physics2D.IgnoreLayerCollision(7, 8);
            Physics2D.IgnoreLayerCollision(8, 8);
            Physics2D.IgnoreLayerCollision(7, 9);
            Physics2D.IgnoreLayerCollision(8, 9);
            Physics2D.IgnoreLayerCollision(9, 9);
        }

        public void LoadQuest(QuestInfo info)
        {
            Info = info;

            Environment = EnvironmentPool.Spawn(Info.Environment);
        }

        public GameInitialize Initialize() 
        {
            Player = Player ?? new Player();

            Recycle();

            var character = PlayerPool.Spawn(Info.Player, 0);

            Player.Set(character);

            Environment.Set(Player);

            Report.InitTime(Info.GameTime);

            Await = new(Info.Enemies);

            if (Info.UseSeeds) 
            {
                _Random.SetSeeds(Info.Seeds);

                GUIDs = _Random.UniqueArray(0, Await.Count, Await.Count, true).ToList();
            }
            
            return new(Player.Tag, Player.GUID, Report.GameTime);
        }

        public void StartLoop() 
        {
            Player.Enable();
        }

        public void Looping() 
        {
            OnScene = CheckEnemies().ToHashSet(); 
        }

        public bool GameOver() 
        {
            var result = false;
            
            if (Player.Dead.Value) { result = true; }

            else if (OnScene.All(e => e.IsDead) && !Await.Any()) { result = true; }

            else if (Report.GameOver) { result = true; }

            if (result) 
            {
                Player.Disable();
            }

            return result;
        }

        public void Recycle() 
        {
            if (Player.Character)
            {
                PlayerPool.Despawn(Player.Release());
            }

            if (OnScene.Any())
            {
                OnScene.ForEach(e => Recycle(e));

                OnScene.Clear();
            }
        }

        private IEnumerable<Enemy> CheckEnemies() 
        {
            if (OnScene.Any(e => !e.IsDead) && !Player.Dead.Value) { Report.UpdateTime(); }

            foreach (var enemy in OnScene) 
            {
                if (enemy.IsDead) 
                {
                    Report.Gather(enemy.Gathered());
                }

                if (enemy.CanRelease) 
                {
                    Recycle(enemy);

                    continue;
                }

                yield return enemy;
            }

            if (!Await.Any()) { yield break; }

            if (Spawn <= 0) 
            {
                Spawn = Info.SpawnTime;

                yield return GetEnemy(Await.Dequeue());
            }

            Spawn -= Time.fixedDeltaTime;
        }

        private Enemy GetEnemy(string characterId) 
        {
            var enemy     = StandBy.Any() ? StandBy.Dequeue() : new(Player, ItemPool);
            var character = EnemyPool.Spawn(characterId, Info.UseSeeds ? GUIDs[_GUID++] : _GUID++);

            enemy.Set(character);
            
            Environment.Set(enemy);

            enemy.Enable();
            
            return enemy;
        }

        private void Recycle(Enemy enemy) 
        {
            var character = enemy.Release();

            enemy.Disable();

            StandBy.Enqueue(enemy);

            EnemyPool.Despawn(character);
        }
    }

    public struct GameInitialize
    {
        public GameInitialize(string tag, int guid, TimeSpan gameTime)
        {
            PlayerTag = tag;
            GUID      = guid;
            GameTime  = gameTime;
        }

        public string   PlayerTag { get; }
        public int      GUID      { get; }
        public TimeSpan GameTime  { get; }
    }

    public struct GameResult 
    {
        public GameResult(bool result, TimeSpan passTime, float injured, int gather) 
        {
            Result   = result;
            PassTime = passTime;
            Injured  = injured;
            Gather   = gather;
        }

        public bool     Result   { get; }
        public TimeSpan PassTime { get; }
        public float    Injured  { get; }
        public int      Gather   { get; }
    }

    public struct OnGather
    {
        public OnGather(int total, int gather)
        {
            Total = total;
            Gather = gather;
        }

        public int Total { get; }
        public int Gather { get; }
    }
}
