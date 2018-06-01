using System;

namespace DreamCube.Framework.DataAccess.Basic.Config
{
    /// <summary>
    /// 数据库提供工厂配置节点类
    /// </summary>
    internal class DBFactoryElement
    {
        #region "配置文件的节点属性"

        /// <summary>
        /// 指定的数据库提供工厂名字
        /// </summary>
        public String ProviderName
        {
            get;
            set;
        }

        /// <summary>
        /// 指定的数据库提供工厂的类型标志字符串，必须对应枚举DBProviderType的值
        /// </summary>
        public String ProviderType
        {
            get;
            set;
        }
        
        /// <summary>
        /// 数据库提供工厂的类型
        /// </summary>
        public String DBFactoryType
        {
            get;
            set;
        }

        #endregion
    }
}
