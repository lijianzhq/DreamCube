//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LY.MQCS.Plugin.DBService.Bak.PQ
{
    using System;
    using System.Collections.Generic;
    
    
    /// <summary>
    ///车辆建付精度数据表
    /// </summary>
    public partial class T_PQ_BU_V_BUILD_PREC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_PQ_BU_V_BUILD_PREC()
        {
            this.T_PQ_BU_V_BUILD_PREC_D = new HashSet<T_PQ_BU_V_BUILD_PREC_D>();
        }
    
        
    /// <summary>
    ///车辆建付精度数据ID
    /// </summary>
    public string V_BUILD_PREC_ID { get; set; }
        
    /// <summary>
    ///测量日期
    /// </summary>
    public System.DateTime MEASURE_DATE { get; set; }
        
    /// <summary>
    ///测量人
    /// </summary>
    public string MEASURE_MAN { get; set; }
        
    /// <summary>
    ///设备编码
    /// </summary>
    public string EQUIP_CODE { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///车间编码
    /// </summary>
    public string WORKSHOP_CODE { get; set; }
        
    /// <summary>
    ///生产线编码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
        
    /// <summary>
    ///车型编码
    /// </summary>
    public string CAR_TYPE_CODE { get; set; }
        
    /// <summary>
    ///VIN码
    /// </summary>
    public string VIN { get; set; }
        
    /// <summary>
    ///CCR编号
    /// </summary>
    public string CCR_CODE { get; set; }
        
    /// <summary>
    ///综合判断
    /// </summary>
    public string COMPREH_JUDGE { get; set; }
        
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_PQ_BU_V_BUILD_PREC_D> T_PQ_BU_V_BUILD_PREC_D { get; set; }
    }
}