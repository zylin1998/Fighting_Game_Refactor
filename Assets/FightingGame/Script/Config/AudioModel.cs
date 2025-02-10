using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Loyufei;
using Zenject;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

namespace FightingGame
{
    public class AudioModel : PropertyModel
    {
        public AudioModel(GlobalDataAccess dataAccess, [Inject(Id = "System")] ConfigProperty file, AudioMixer audioMixer)
        {
            AudioMixer = audioMixer;

            Root = new GameObject("AudioMixer").transform;

            _StandardVolumn = new[]
            {
                new Property<float>("Master", 1f),
                new Property<float>("BGM"   , 1f),
                new Property<float>("SE"    , 1f),
                new Property<float>("SFX"   , 1f),
            }.ToDictionary(k => k.Id);

            _StandardMutes = new[]
            {
                new Property<bool>("Master", false),
                new Property<bool>("BGM"   , false),
                new Property<bool>("SE"    , false),
                new Property<bool>("SFX"   , false),
            }.ToDictionary(k => k.Id);

            Volumns = _StandardVolumn.Values.ToDictionary(k => k.Id, v => file.GetFloat  (v.Id, v.Value));
            Mutes   = _StandardMutes .Values.ToDictionary(k => k.Id, v => file.GetBoolean(v.Id, v.Value));
            
            Sources = AudioMixer.FindMatchingGroups(string.Empty).ToDictionary(
                k => (object)k.name,
                v => GetSource(v));

            foreach (var p in Volumns.Values) { dataAccess.Install(p); }
            foreach (var p in Mutes  .Values) { dataAccess.Install(p); }

            dataAccess.Install(this);
        }

        private AudioClip _Select;
        private AudioClip _Click;

        private Dictionary<object, Property<float>> _StandardVolumn;
        private Dictionary<object, Property<bool>>  _StandardMutes;

        private Transform Root { get; }

        public AudioMixer AudioMixer { get; }
        public Dictionary<object, AudioSource>     Sources     { get; }
        public Dictionary<object, Property<float>> Volumns     { get; }
        public Dictionary<object, Property<bool>>  Mutes       { get; }

        public void SetSelectableClip(AudioClip select, AudioClip click) 
        {
            _Select = select;
            _Click  = click;
        }

        public void BindSelectables(Selectable selectable) 
        {
            selectable.OnPointerEnterAsObservable().Subscribe((e) => Play("SE", _Select));
            selectable.OnPointerClickAsObservable().Subscribe((e) => Play("SE", _Click));
        }

        public void Initialize() 
        {
            foreach (var p in Volumns.Values) { Set((string)p.Id, p.Value); }
            foreach (var p in Mutes  .Values) { Set((string)p.Id, p.Value); }
        }
        
        public void Play(string name, AudioClip clip) 
        {
            if (!Sources.TryGetValue(name, out var source)) { return; }

            source.PlayOneShot(clip);
        }

        public void PlayLoop(string name, AudioClip clip)
        {
            if (!Sources.TryGetValue(name, out var source)) { return; }

            source.clip = clip;
            source.loop = true;

            source.Play();
        }

        public void Set(string name, float volumn, bool force = false) 
        {
            volumn = volumn.Clamp01();
            
            if (!Volumns.TryGetValue(name, out var p) && p.Value == volumn && !force) { return; }
            
            p.Set(volumn);

            var result = volumn <= 0 || Mutes[name].Value ? -80 : Mathf.Log10(volumn) * 20;
            
            AudioMixer.SetFloat(name, result);
        }

        public void Set(string name, bool mute, bool force = false)
        {
            if (!Mutes.TryGetValue(name, out var m) && m.Value == mute && !force) { return; }
            
            m.Set(mute);

            var result = mute ? -80f : Mathf.Log10(Volumns[name].Value) * 20;
            
            AudioMixer.SetFloat(name, result);
        }

        private AudioSource GetSource(AudioMixerGroup group) 
        {
            var source = new GameObject(group.name).AddComponent<AudioSource>();

            source.outputAudioMixerGroup = group;

            source.transform.SetParent(Root);
            
            return source;
        }
    }
}
