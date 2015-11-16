using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Utility
{
    /// <summary>
    /// 枚举辅助类
    /// </summary>
    public class EnumHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumField"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumField)
        {
            return GetEnumItem(enumField).Description;
        }

        public static T GetEnumObject<T>(string text)
        {
            return (T)Enum.Parse(typeof(T), text, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumField"></param>
        /// <returns></returns>
        public static EnumItem GetEnumItem<T>(T enumField)
        {
            EnumItemCollection enumItems = GetEnumItems(typeof(T));
            return enumItems[enumField.ToString()];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumField"></param>
        /// <returns></returns>
        public static EnumItem GetEnumItem(Enum enumField)
        {
            EnumItemCollection enumItems = GetEnumItems(enumField.GetType());
            return enumItems[enumField.ToString()];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static EnumItemCollection GetEnumItems(Type enumType)
        {
            return EnumItemsCache.Get(enumType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetEnumByKey<T>(string key)
        {
            return GetEnumByKey<T>(key, default(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetEnumByKey<T>(string key, T defaultValue)
        {
            T item = defaultValue;
            EnumItem enumItem = null;
            try
            {
                Type enumType = typeof(T);
                EnumItemCollection enumItems = EnumItemsCache.Get(enumType);
                enumItems.TryGet(key, out enumItem);
                if (enumItem != null)
                {
                    item = (T)Enum.ToObject(enumType, enumItem.Value);
                }
            }
            catch
            {
                item = defaultValue;
            }
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetEnumByValue<T>(int value)
        {
            return GetEnumByValue<T>(value, default(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetEnumByValue<T>(int value, T defaultValue)
        {
            T item = defaultValue;
            try
            {
                EnumItemCollection enumItems = GetEnumItems(typeof(T));
                foreach (EnumItem enumItem in enumItems)
                {
                    if (enumItem.Value == value)
                    {
                        item = (T)Enum.ToObject(typeof(T), enumItem.Value);
                    }
                }
            }
            catch
            {
                item = defaultValue;
            }
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetEnumByValue<T>(string value)
        {
            return GetEnumByValue<T>(value, default(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetEnumByValue<T>(string value, T defaultValue)
        {
            T item = defaultValue;
            try
            {
                EnumItemCollection enumItems = GetEnumItems(typeof(T));
                foreach (EnumItem enumItem in enumItems)
                {
                    if (enumItem.Value.ToString() == value)
                    {
                        item = (T)Enum.ToObject(typeof(T), enumItem.Value);
                    }
                }
            }
            catch
            {
                item = defaultValue;
            }
            return item;
        }

        /// <summary>
        /// get the Enum value according to the its decription
        /// </summary>
        /// <typeparam name="T">the type of the enum</typeparam>
        /// <param name="description">the description of the EnumValue</param>
        /// <returns></returns>
        public static T GetEnumByDescription<T>(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return default(T);
            }

            Type enumType = typeof(T);
            EnumItemCollection list = GetEnumItems(enumType);
            foreach (EnumItem item in list)
            {
                if (item.Description.ToString().ToLower() == description.Trim().ToLower())
                {
                    return (T)Enum.ToObject(typeof(T), item.Value);
                }
            }
            return default(T);
        }
    }

    /// <summary>
    /// RelationShip between Key and Value
    /// </summary>
    public class EnumItem
    {
        private string _key;
        private int _value;
        private string _description;


        /// <summary>
        /// Enum Key
        /// </summary>
        public string Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Enum Value
        /// </summary>
        public int Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Enum Description
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Custructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public EnumItem(string key, int value)
            : this(key, value, key)
        {
        }

        /// <summary>
        /// Custructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public EnumItem(string key, int value, string description)
        {
            _key = key;
            _value = value;
            _description = description;
        }
    }

    public class EnumItemCollection : KeyedCollection<string, EnumItem>
    {
        protected override string GetKeyForItem(EnumItem item)
        {
            return item.Key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="enumItem"></param>
        /// <returns></returns>
        public bool TryGet(string key, out EnumItem enumItem)
        {
            enumItem = null;
            if (this.Contains(key))
            {
                enumItem = this[key];
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyExist(string key)
        {
            bool isExists = false;
            foreach (EnumItem enumItem in this)
            {
                if (key == enumItem.Key)
                    isExists = true;
            }
            return isExists;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EnumItemsCache
    {
        private static Dictionary<Type, EnumItemCollection> _typedEnumItemsCache
            = new Dictionary<Type, EnumItemCollection>();
        private static object _syncObj = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static EnumItemCollection Get(Type enumType)
        {
            EnumItemCollection enumItems = null;
            if (!_typedEnumItemsCache.TryGetValue(enumType, out enumItems))
            {
                enumItems = GetEnumItems(enumType);
                Add(enumType, enumItems);
            }

            return enumItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="enumItems"></param>
        private static void Add(Type enumType, EnumItemCollection enumItems)
        {
            if (!_typedEnumItemsCache.ContainsKey(enumType))
            {
                lock (_syncObj)
                {
                    if (!_typedEnumItemsCache.ContainsKey(enumType))
                    {
                        _typedEnumItemsCache.Add(enumType, enumItems);
                    }
                }
            }
        }

        /// <summary>
        /// get the enum's all list
        /// </summary>
        /// <param name="enumType">the type of the enum</param>
        /// <param name="withAll">identicate whether the returned list should contain the all item</param>
        /// <returns></returns>
        private static EnumItemCollection GetEnumItems(Type enumType)
        {
            EnumItemCollection emumItems = new EnumItemCollection();

            if (enumType.IsEnum != true)
            {
                // just whethe the type is enum type
                throw new InvalidOperationException();
            }

            // 获得特性Description的类型信息
            Type typeDescription = typeof(DescriptionAttribute);

            // 获得枚举的字段信息（因为枚举的值实际上是一个static的字段的值）
            FieldInfo[] fields = enumType.GetFields();

            // 检索所有字段
            foreach (FieldInfo field in fields)
            {
                // 过滤掉一个不是枚举值的，记录的是枚举的源类型
                if (field.FieldType.IsEnum == false)
                {
                    continue;
                }

                // 通过字段的名字得到枚举的值
                int value = (int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                string description = string.Empty;

                // 获得这个字段的所有自定义特性，这里只查找Description特性
                object[] arr = field.GetCustomAttributes(typeDescription, true);
                if (arr.Length > 0)
                {
                    // 因为Description自定义特性不允许重复，所以只取第一个
                    DescriptionAttribute descriptionAttri = (DescriptionAttribute)arr[0];

                    // 获得特性的描述值
                    description = descriptionAttri.Description;
                }
                else
                {
                    // 如果没有特性描述，那么就显示英文的字段名
                    description = field.Name;
                }
                emumItems.Add(new EnumItem(field.Name, value, description));
            }

            return emumItems;
        }
    }
}
