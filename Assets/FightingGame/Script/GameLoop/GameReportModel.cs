using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace FightingGame
{
    public class GameReportModel
        : PropertyModel, IObservable<TimeSpan>, IObservable<OnGather>
    {
        public GameReportModel(GlobalDataAccess dataAccess)
        {
            Time     = new StandardProperty<float>("GameTime");
            Gathered = new Property<int>("Gather");
            Injured  = new Property<float>("Injured");

            dataAccess.Install(Time);
            dataAccess.Install(Gathered);
            dataAccess.Install(this);
        }

        private Subject<TimeSpan> _OnTimeChange = new();
        private Subject<OnGather> _OnGathered   = new();

        public StandardProperty<float> Time { get; }
        public Property<int>   Gathered { get; }
        public Property<float> Injured  { get; }

        public TimeSpan GameTime => TimeSpan.FromSeconds(Time.Standard - Time.Value);
        public TimeSpan LeftTime => TimeSpan.FromSeconds(Time.Value);

        public bool  GameOver     => Time.Value <= 0;
        public int   TotalGather  => Gathered.Value;
        public float TotalInjured => Injured.Value;

        public void UpdateTime()
        {
            Time.Set(Time.Value - UnityEngine.Time.fixedDeltaTime);

            _OnTimeChange.OnNext(LeftTime);
        }

        public void InitTime(float time)
        {
            Time.SetStandard(time);
            Time.Reset();
        }

        public void ResetTime()
        {
            Time.Reset();
        }

        public int Gather(int gather)
        {
            if (gather <= 0) { return 0; }

            Gathered.Set(Gathered.Value + gather);

            _OnGathered.OnNext(new(Gathered.Value, gather));

            return Gathered.Value;
        }

        public float Injure(float damage)
        {
            Injured.Set(Injured.Value + damage);

            return Injured.Value;
        }

        public IDisposable Subscribe(IObserver<TimeSpan> observer)
        {
            return _OnTimeChange.Subscribe(observer);
        }

        public IDisposable Subscribe(IObserver<OnGather> observer)
        {
            return _OnGathered.Subscribe(observer);
        }
    }
}
