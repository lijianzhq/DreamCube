//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LY.MQCS.Plugin.DBService.Bak.DB
{
    using System;
    using System.Collections.Generic;
    
    
    /// <summary>
    ///大区信息
    /// </summary>
    public partial class T_DB_DB_BIG_AREA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_DB_DB_BIG_AREA()
        {
            this.T_DB_DB_SMALL_AREA = new HashSet<T_DB_DB_SMALL_AREA>();
        }
    
        
    /// <summary>
    ///大区ID
    /// </summary>
    public string BIG_AREA_ID { get; set; }
        
    /// <summary>
    ///大区编码
    /// </summary>
    public string BIG_AREA_CODE { get; set; }
        
    /// <summary>
    ///大区名称
    /// </summary>
    public string BIG_AREA_NAME { get; set; }
        
    /// <summary>
    ///车辆品牌编码
    /// </summary>
    public string CAR_BRAND_CODE { get; set; }
        
    /// <summary>
    ///排列顺序
    /// </summary>
    public decimal ORDER_NO { get; set; }
        
    /// <summary>
    ///是否可用
    /// </summary>
    public string IS_ENABLE { get; set; }
        
    /// <summary>
    ///创建人
    /// </summary>
    public string CREATOR { get; set; }
        
    /// <summary>
    ///创建日期
    /// </summary>
    public System.DateTime CREATED_DATE { get; set; }
        
    /// <summary>
    ///最后更新人员
    /// </summary>
    public string MODIFIER { get; set; }
        
    /// <summary>
    ///最后更新时间
    /// </summary>
    public System.DateTime LAST_UPDATED_DATE { get; set; }
        
    /// <summary>
    ///SDP用户ID
    /// </summary>
    public string SDP_USER_ID { get; set; }
        
    /// <summary>
    ///SDP组织ID
    /// </summary>
    public string SDP_ORG_ID { get; set; }
        
    /// <summary>
    ///并发控制字段
    /// </summary>
    public string UPDATE_CONTROL_ID { get; set; }
    
        public virtual T_DB_DB_AREA T_DB_DB_AREA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_DB_DB_SMALL_AREA> T_DB_DB_SMALL_AREA { get; set; }
    }
}