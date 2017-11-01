using System;

namespace DreamCube.Foundation.Basic.Objects
{
    /// <summary>
    /// 表示枚举类型的每一项枚举值
    /// </summary>
    public class EnumItem
    {
        /// <summary>
        /// 枚举项的Description值
        /// </summary>
        public String Description
        { get; set; }

        /// <summary>
        /// 枚举项对应的枚举值
        /// </summary>
        public Object EnumValue
        { get; set; }

        /// <summary>
        /// 枚举项的名称
        /// </summary>
        public String IdentityValue
        { get; set; }

        /// <summary>
        /// 枚举的基础类型
        /// </summary>
        public Type UnderlyingType
        { get; set; }

        /// <summary>
        /// 枚举的基础类型对应的值
        /// </summary>
        public Object UnderlyingValue
        { get; set; }
    }

    /// <summary>
    /// 表示枚举类型的每一项枚举值
    /// </summary>
    public class EnumItem<T> : EnumItem
    { }
}
