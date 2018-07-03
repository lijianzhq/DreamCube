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
    ///发动机设备测量主信息
    /// </summary>
    public partial class T_PQ_BU_EQUIP_MEASURE_M_E
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_PQ_BU_EQUIP_MEASURE_M_E()
        {
            this.T_PQ_BU_EQUIP_MEASURE_E_V = new HashSet<T_PQ_BU_EQUIP_MEASURE_E_V>();
        }
    
        
    /// <summary>
    ///发动机设备测量主信息ID
    /// </summary>
    public string EQUIP_MEASURE_M_E_ID { get; set; }
        
    /// <summary>
    ///设备测量编码
    /// </summary>
    public string EQUIP_MEASURE_CODE { get; set; }
        
    /// <summary>
    ///设备编码
    /// </summary>
    public string EQUIP_CODE { get; set; }
        
    /// <summary>
    ///工艺编码
    /// </summary>
    public string CRAFT_CODE { get; set; }
        
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
    ///工序编码
    /// </summary>
    public string PROCESS_CODE { get; set; }
        
    /// <summary>
    ///车系编码
    /// </summary>
    public string CAR_SERIES_CODE { get; set; }
        
    /// <summary>
    ///车型编码
    /// </summary>
    public string CAR_TYPE_CODE { get; set; }
        
    /// <summary>
    ///VIN码
    /// </summary>
    public string VIN { get; set; }
        
    /// <summary>
    ///零件编码
    /// </summary>
    public string PART_NO { get; set; }
        
    /// <summary>
    ///测量人
    /// </summary>
    public string MEASURE_MAN { get; set; }
        
    /// <summary>
    ///测量时间
    /// </summary>
    public System.DateTime MEASURE_DATE { get; set; }
        
    /// <summary>
    ///判断结果
    /// </summary>
    public string JUDGE_RESULT { get; set; }
        
    /// <summary>
    ///备注
    /// </summary>
    public string REMARK { get; set; }
        
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
        public virtual ICollection<T_PQ_BU_EQUIP_MEASURE_E_V> T_PQ_BU_EQUIP_MEASURE_E_V { get; set; }
    }
}