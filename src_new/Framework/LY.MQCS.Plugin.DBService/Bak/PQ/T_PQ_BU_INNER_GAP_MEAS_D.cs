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
    ///内间隙测量数据明细表
    /// </summary>
    public partial class T_PQ_BU_INNER_GAP_MEAS_D
    {
        
    /// <summary>
    ///内间隙测量数据明细表ID
    /// </summary>
    public string INNER_GAP_MEAS_D_ID { get; set; }
        
    /// <summary>
    ///位置ID
    /// </summary>
    public string POS_ID { get; set; }
        
    /// <summary>
    ///位置名称
    /// </summary>
    public string POS_NAME { get; set; }
        
    /// <summary>
    ///RL
    /// </summary>
    public string RL_VAL { get; set; }
        
    /// <summary>
    ///方向
    /// </summary>
    public string DIRECTION { get; set; }
        
    /// <summary>
    ///间隙
    /// </summary>
    public decimal GAP_VAL { get; set; }
        
    /// <summary>
    ///面差
    /// </summary>
    public decimal FACE_VAL { get; set; }
        
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
    
        public virtual T_PQ_BU_INNER_GAP_MEAS T_PQ_BU_INNER_GAP_MEAS { get; set; }
    }
}