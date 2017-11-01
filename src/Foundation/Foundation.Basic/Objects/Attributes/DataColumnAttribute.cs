using System;

namespace DreamCube.Foundation.Basic.Objects.Attributes
{
    /// <summary>
    /// 属性上面打标签，绑定属性与数据库列的关系
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DataColumnAttribute : Attribute
    {
        /// <summary>
        /// 数据库列名
        /// </summary>
        public String DBColumnName
        {
            get;
            set;
        }
    }
}
