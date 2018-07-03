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
    ///整车设备测量明细信息
    /// </summary>
    public partial class T_PQ_BU_EQUIP_MEASURE_S_V
    {
        
    /// <summary>
    ///整车设备测量明细信息ID
    /// </summary>
    public string EQUIP_MEASURE_S_V_ID { get; set; }
        
    /// <summary>
    ///测量项目
    /// </summary>
    public string MEASURE_ITEM { get; set; }
        
    /// <summary>
    ///检测部位
    /// </summary>
    public string CHECK_PART_POS_CODE { get; set; }
        
    /// <summary>
    ///检测点位
    /// </summary>
    public string CHECK_POINT { get; set; }
        
    /// <summary>
    ///标准值-下限值
    /// </summary>
    public decimal STAN_VAL_MIN { get; set; }
        
    /// <summary>
    ///标准值-上限值
    /// </summary>
    public decimal STAN_VAL_MAX { get; set; }
        
    /// <summary>
    ///标准值-文本
    /// </summary>
    public string STAN_VAL_TEXT { get; set; }
        
    /// <summary>
    ///测量值
    /// </summary>
    public string MEASURE_VAL { get; set; }
        
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
    
        public virtual T_PQ_BU_EQUIP_MEASURE_M_V T_PQ_BU_EQUIP_MEASURE_M_V { get; set; }
    }
}
