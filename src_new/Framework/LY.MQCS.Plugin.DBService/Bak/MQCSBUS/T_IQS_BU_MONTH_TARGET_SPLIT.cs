//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LY.MQCS.Plugin.DBService.Bak.MQCSBUS
{
    using System;
    using System.Collections.Generic;
    
    
    /// <summary>
    ///IQS月份目标分解
    /// </summary>
    public partial class T_IQS_BU_MONTH_TARGET_SPLIT
    {
        
    /// <summary>
    ///IQS月份目标分解ID
    /// </summary>
    public string MONTH_TARGET_SPLIT_ID { get; set; }
        
    /// <summary>
    ///类型
    /// </summary>
    public string TYPE { get; set; }
        
    /// <summary>
    ///年月
    /// </summary>
    public string YEAR_MONTH { get; set; }
        
    /// <summary>
    ///车系编码
    /// </summary>
    public string CAR_SERIES_CODE { get; set; }
        
    /// <summary>
    ///责任单位代码
    /// </summary>
    public string DUTY_COMP_CODE { get; set; }
        
    /// <summary>
    ///月份目标值
    /// </summary>
    public decimal MONTH_TARGET { get; set; }
        
    /// <summary>
    ///月份挑战值
    /// </summary>
    public decimal MONTH_CHALLENGE { get; set; }
        
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
    ///创建时间
    /// </summary>
    public System.DateTime CREATED_DATE { get; set; }
        
    /// <summary>
    ///修改人
    /// </summary>
    public string MODIFIER { get; set; }
        
    /// <summary>
    ///修改时间
    /// </summary>
    public System.DateTime LAST_UPDATED_DATE { get; set; }
        
    /// <summary>
    ///并发控制字段
    /// </summary>
    public string UPDATE_CONTROL_ID { get; set; }
        
    /// <summary>
    ///SDP用户ID
    /// </summary>
    public string SDP_USER_ID { get; set; }
        
    /// <summary>
    ///SDP组织ID
    /// </summary>
    public string SDP_ORG_ID { get; set; }
        
    /// <summary>
    ///BI指标
    /// </summary>
    public string BI { get; set; }
    }
}