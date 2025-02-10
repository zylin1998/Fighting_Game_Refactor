using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ExcelDataReader;
using Zenject;

namespace FightingGame
{
    public class LanguageModel : PropertyModel, IObservable<IEnumerable<LanguageModel.Context>>
    {
        public LanguageModel(GlobalDataAccess dataAccess, [Inject(Id = "System")] ConfigProperty file) 
        {
            _Standard = new Property<int>("Language", GetSystemLanguageIndex());

            Property = file.GetInteger(_Standard.Id, _Standard.Value);

            var path = Path.Combine(Application.streamingAssetsPath, "Excel", "語言文本.xlsx");

            Reader = new ExcelReader(path);

            dataAccess.Install(Property);

            dataAccess.Install(this);
        }

        private Property<int> _Standard;

        private Subject<IEnumerable<Context>> _Subject = new();

        public object this[object uuid, int others] 
            => Contexts.FirstOrDefault((p) => p.UUID.Equals(uuid))?[others];

        public Property<int> Property { get; }

        public ExcelReader Reader { get; }

        public Context[] Contexts { get; private set; }

        public void Set(int index) 
        {
            var result = index % Supported.Length;

            if (result == Property.Value) { return; }

            Property.Set(result);
        }

        public void Read() 
        {
            var index = Property.Value;

            if (!Reader.TryRead(Supported[index].ToString(), out var read)) { return; }

            Contexts = read.Parse(Parse).ToArray();

            _Subject.OnNext(Contexts);
        }

        public IDisposable Subscribe(IObserver<IEnumerable<Context>> observer) 
        {
            return _Subject.Subscribe(observer);
        }

        private Context Parse(DataRow row) 
        {
            var context = new Context();

            for (var index = 0; index < row.ItemArray.Length; index++) 
            {
                if (index == 0) { context.UUID  = row[0]; }
                
                if (index == 1) { context.Title = row[1]; }

                if (index >= 2) 
                {
                    context.Add(row[index]);
                }
            }

            return context;
        }

        [Serializable]
        public class Context
        {
            [SerializeField]
            private string _UUID;
            [SerializeField]
            private string _Title;

            private List<string> _Others = new();

            public object UUID  { get =>  _UUID; set =>  _UUID = value.ToString(); }
            public object Title { get => _Title; set => _Title = value.ToString(); }

            public object this[int index] => index < _Others.Count ? _Others[index] : string.Empty;

            public void Add(object other) => _Others.Add(other.ToString());
        } 

        public static SystemLanguage[] Supported = new[] { SystemLanguage.ChineseTraditional, SystemLanguage.English };

        public static int GetSystemLanguageIndex() 
        {
            var language = Application.systemLanguage;

            for (var i = 0; i < Supported.Length; i++) 
            {
                if (Supported[i] == language) { return i; }
            }

            return 1;
        }
    }

    public class ExcelReader 
    {
        public ExcelReader(string path) 
        {
            Path = path;
        }

        public string Path { get; }

        public DataRowCollection Read(string sheet) 
        {
            using (FileStream fileStream = File.Open(Path, FileMode.Open, FileAccess.Read)) 
            {
                var reader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                var result = reader.AsDataSet();

                return result.Tables[sheet]?.Rows;
            }
        }

        public bool TryRead(string sheet, out DataRowCollection dataRow)
        {
            using (FileStream fileStream = File.Open(Path, FileMode.Open, FileAccess.Read))
            {
                var reader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                var result = reader.AsDataSet();

                dataRow = result.Tables[sheet]?.Rows;

                return dataRow != default;
            }
        }
    }

    public static class ExcelReaderExtensions
    {
        public static IEnumerable<T> Parse<T>(this DataRowCollection self, Func<DataRow, T> parse) 
        {
            foreach (DataRow row in self) 
            {
                yield return parse(row);
            }
        }
    }
}
