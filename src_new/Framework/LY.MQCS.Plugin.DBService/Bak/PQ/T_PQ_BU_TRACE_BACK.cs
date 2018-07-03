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
    ///追溯问题信息
    /// </summary>
    public partial class T_PQ_BU_TRACE_BACK
    {
        
    /// <summary>
    ///追溯问题信息ID
    /// </summary>
    public string TRACE_BACK_ID { get; set; }
        
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
    ///发动机号
    /// </summary>
    public string ENGINE_NO { get; set; }
        
    /// <summary>
    ///问题类型
    /// </summary>
    public string PRO_TYPE { get; set; }
        
    /// <summary>
    ///问题描述
    /// </summary>
    public string PROBLEM_DESC { get; set; }
        
    /// <summary>
    ///问题反馈人
    /// </summary>
    public string PROBLEM_FB_MAN { get; set; }
        
    /// <summary>
    ///反馈时间
    /// </summary>
    public System.DateTime FEEDBACK_DATE { get; set; }
        
    /// <summary>
    ///原因类型
    /// </summary>
    public string REASON_TYPE { get; set; }
        
    /// <summary>
    ///原因分析
    /// </summary>
    public string REASON_ANALYSIS { get; set; }
        
    /// <summary>
    ///调查人
    /// </summary>
    public string INVEST_MAN { get; set; }
        
    /// <summary>
    ///调查日期
    /// </summary>
    public System.DateTime INVEST_DATE { get; set; }
        
    /// <summary>
    ///调查内容
    /// </summary>
    public string INVEST_CONTENT { get; set; }
        
    /// <summary>
    ///调查结果
    /// </summary>
    public string INVEST_RESULT { get; set; }
        
    /// <summary>
    ///是否批次问题
    /// </summary>
    public string IS_BATCH_PROB { get; set; }
        
    /// <summary>
    ///对策内容
    /// </summary>
    public string CM_CONTENT { get; set; }
        
    /// <summary>
    ///对策人
    /// </summary>
    public string CM_MAN { get; set; }
        
    /// <summary>
    ///对策日期
    /// </summary>
    public System.DateTime CM_DATE { get; set; }
        
    /// <summary>
    ///对策结果
    /// </summary>
    public string CM_RESULT { get; set; }
        
    /// <summary>
    ///追溯问题状态
    /// </summary>
    public string TRACE_BACK_STATUS { get; set; }
        
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
    }
}
