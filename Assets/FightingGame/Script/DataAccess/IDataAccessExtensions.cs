using System;
using System.Linq;
using System.Collections.Generic;

namespace FightingGame
{
    public static class IDataAccessExtensions
    {
        #region Install

        public static void Install(this IDataAccess self, UnityEngine.Object obj)
        {
            self.Assets.Add(obj);
        }

        public static void Install(this IDataAccess self, object obj)
        {
            self.Objects.Add(obj);
        }

        public static void Install(this IDataAccess self, Property<int> property)
        {
            self.Properties.Add(property);
        }

        public static void Install(this IDataAccess self, Property<float> property)
        {
            self.Properties.Add(property);
        }

        public static void Install(this IDataAccess self, Property<bool> property)
        {
            self.Properties.Add(property);
        }

        public static void Install(this IDataAccess self, PropertyModel model)
        {
            self.Properties.Add(model);
        }

        #endregion

        #region Uninstall

        public static bool Uninstall(this IDataAccess self, UnityEngine.Object obj)
        {
            return self.Assets.Remove(obj);
        }

        public static bool Uninstall(this IDataAccess self, object obj)
        {
            return self.Objects.Remove(obj);
        }

        public static bool Uninstall(this IDataAccess self, Property<int> property)
        {
            return self.Properties.Remove(property);
        }

        public static bool Uninstall(this IDataAccess self, Property<float> property)
        {
            return self.Properties.Remove(property);
        }

        public static bool Uninstall(this IDataAccess self, Property<bool> property)
        {
            return self.Properties.Remove(property);
        }

        public static bool Uninstall(this IDataAccess self, PropertyModel model)
        {
            return self.Properties.Remove(model);
        }

        #endregion

        public static T GetAsset<T>(this IDataAccess self)
        {
            return self.Assets.Get<T>();
        }

        public static T GetAsset<T>(this IDataAccess self, string name)
        {
            return self.Assets.GetByName<T>(name);
        }

        public static IEnumerable<T> GetAssetAll<T>(this IDataAccess self)
        {
            return self.Assets.GetAll<T>();
        }

        public static Property<int> GetInteger(this IDataAccess self, string id)
        {
            return self.Properties.GetInteger(id);
        }

        public static Property<float> GetFloat(this IDataAccess self, string id)
        {
            return self.Properties.GetFloat(id);
        }

        public static Property<bool> GetBoolean(this IDataAccess self, string id)
        {
            return self.Properties.GetBoolean(id);
        }

        public static T GetModel<T>(this IDataAccess self) where T : PropertyModel
        {
            return self.Properties.GetModel<T>();
        }

        public static T GetProperty<T>(this IDataAccess self)
        {
            return self.Properties.Get<T>();
        }

        public static IEnumerable<T> GetPropertyAll<T>(this IDataAccess self)
        {
            return self.Properties.GetAll<T>();
        }

        public static T Get<T>(this IDataAccess self)
        {
            return self.Objects.Get<T>();
        }
    }
}
