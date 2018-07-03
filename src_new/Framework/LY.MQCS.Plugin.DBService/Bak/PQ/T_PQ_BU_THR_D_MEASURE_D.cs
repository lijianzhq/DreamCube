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
    ///3D测量数据明细表
    /// </summary>
    public partial class T_PQ_BU_THR_D_MEASURE_D
    {
        
    /// <summary>
    ///3D测量数据明细表ID
    /// </summary>
    public string THR_D_MEASURE_D_ID { get; set; }
        
    /// <summary>
    ///位置ID
    /// </summary>
    public string POS_ID { get; set; }
        
    /// <summary>
    ///位置名称
    /// </summary>
    public string POS_NAME { get; set; }
        
    /// <summary>
    ///方向
    /// </summary>
    public string DIRECTION { get; set; }
        
    /// <summary>
    ///3D测量标示
    /// </summary>
    public string MEAS_MARK_3D { get; set; }
        
    /// <summary>
    ///3D测量值
    /// </summary>
    public decimal MEAS_VAL_3D { get; set; }
        
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
    
        public virtual T_PQ_BU_THR_D_MEASURE T_PQ_BU_THR_D_MEASURE { get; set; }
    }
}
