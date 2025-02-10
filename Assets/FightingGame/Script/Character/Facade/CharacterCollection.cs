using System;
using System.Linq;
using System.Collections.Generic;

namespace FightingGame
{
    public class CharacterCollection
    {
        private Dictionary<object, Group> Groups { get; } = new();

        public void Add(object group, Character character) 
        {
            GetGroup(group).Add(character.GUID, character);
        }

        public bool Remove(object group, Character character)
        {
            return GetGroup(group).Remove(character.GUID);
        }

        public Character Get(object group, object guid) 
        {
            return GetGroup(group).Get(guid);
        }

        private Group GetGroup(object groupId) 
        {
            if (Groups.TryGetValue(groupId, out var group)) 
            {
                return group;
            }

            group = new Group();

            Groups.Add(groupId, group);

            return group;
        }

        private class Group 
        {
            public Dictionary<object, Character> Characters { get; } = new();

            public void Add(object key, Character character) 
            {
                Characters.Add(key, character);
            }

            public bool Remove(object key) 
            {
                return Characters.Remove(key);
            }

            public Character Get(object key) 
            {
                if (Characters.TryGetValue(key, out var character))
                {
                    return character; 
                }
                
                return default;
            }
        }
    }
}
