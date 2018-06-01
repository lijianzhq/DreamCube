using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using DreamCube.Foundation.Basic.Utility;
using DreamCube.Foundation.Serialization;
using DreamCube.Foundation.Serialization.Serialization;

namespace DreamCube.Framework.Utilities.BasicObject
{
    public class ObjectX
    {
        #region "字段"

        private Dictionary<String, Object> items = new Dictionary<String, Object>();

        #endregion

        #region "属性"

        [JsonProperty]
        protected Dictionary<String, Object> Items
        {
            get { return items; }
        }

        /// <summary>
        /// 对象的项数
        /// </summary>
        [JsonIgnore]
        public Int32 ItemCount
        {
            get { return items.Count; }
        }

        #endregion

        #region "方法"

        public ObjectX()
        { }

        /// <summary>
        /// 设置对象的属性值
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="value"></param>
        public void SetItemValue(String itemName, Object value)
        {
            MyDictionary.TryAdd(items, itemName, value, Foundation.Basic.Enums.CollectionsAddOper.ReplaceIfExist);
        }

        /// <summary>
        /// 根据键获取对应的值
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public Object GetItemValue(String itemName, Object defaultValue = null)
        {
            Object value = null;
            if (MyDictionary.TryGetValue(items, itemName, out value, Foundation.Basic.Enums.CollectionsGetOper.DefaultValueIfNotExist, null))
                return value;
            else
                return defaultValue;
        }

        /// <summary>
        /// 根据建获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetItemValue<T>(String itemName, T defaultValue = default(T))
        {
            Object value = null;
            if (MyDictionary.TryGetValue(items, itemName, out value, Foundation.Basic.Enums.CollectionsGetOper.DefaultValueIfNotExist, null))
            {
                if (value != null)
                    return (T)MyConverter.Convert(value, typeof(T));
            }
            return defaultValue;
        }

        /// <summary>
        /// 保存此对象的json格式数据到指定的文件
        /// </summary>
        /// <param name="filePath">指定的文件完整路径</param>
        /// <param name="append">追加写入还是全新写入；默认是</param>
        public void SaveJSONToFile(String filePath, Boolean append = false)
        {
            MyIO.Write(filePath, this.ToJSON(), append, Encoding.UTF8);
        }

        /// <summary>
        /// 把对象的json格式数据保存到注册表中
        /// </summary>
        /// <param name="subKey">注册表项，例如：SOFTWARE\WinRAR</param>
        /// <param name="propertyName">对应注册表项中的属性名</param>
        public void SaveJSONToRegistry_LocalMachine(String subKey, String propertyName)
        {
            MyRegistry.Basic.SetLocalMachineKeyPropertyValue(subKey, propertyName, this.ToJSON(), true);
        }

        /// <summary>
        /// 获取此对象是JSON字符串格式数据
        /// </summary>
        /// <returns></returns>
        public String ToJSON()
        {
            return MyJson.Serialize(this);
        }

        #endregion
    }
}
