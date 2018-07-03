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
    ///IQS责任划分规则历史表
    /// </summary>
    public partial class T_IQS_DB_DUTY_ROLE_HIS
    {
        
    /// <summary>
    ///责任划分规则ID
    /// </summary>
    public string IQS_DUTY_ROLE { get; set; }
        
    /// <summary>
    ///项目编码
    /// </summary>
    public string ITEM_CODE { get; set; }
        
    /// <summary>
    ///项目细分号
    /// </summary>
    public string ITEM_DETAIL_CODE { get; set; }
        
    /// <summary>
    ///车系编码
    /// </summary>
    public string CAR_SERIES_CODE { get; set; }
        
    /// <summary>
    ///责任单位代码1
    /// </summary>
    public string DUTY_COMP_CODE_ONE { get; set; }
        
    /// <summary>
    ///承担比例1
    /// </summary>
    public decimal BEAR_RATIO_ONE { get; set; }
        
    /// <summary>
    ///责任单位代码2
    /// </summary>
    public string DUTY_COMP_CODE_TOW { get; set; }
        
    /// <summary>
    ///承担比例2
    /// </summary>
    public decimal BEAR_RATIO_TOW { get; set; }
        
    /// <summary>
    ///责任单位代码3
    /// </summary>
    public string DUTY_COMP_CODE_THREE { get; set; }
        
    /// <summary>
    ///承担比例3
    /// </summary>
    public decimal BEAR_RATIO_THREE { get; set; }
        
    /// <summary>
    ///变更理由
    /// </summary>
    public string CHANGE_REASON { get; set; }
        
    /// <summary>
    ///提交人
    /// </summary>
    public string SUBMITOR { get; set; }
        
    /// <summary>
    ///提交时间
    /// </summary>
    public System.DateTime SUBMIT_TIME { get; set; }
        
    /// <summary>
    ///生效日期
    /// </summary>
    public System.DateTime EFFECTIVE_DATE { get; set; }
        
    /// <summary>
    ///审核科长
    /// </summary>
    public string SC_AUDITOR { get; set; }
        
    /// <summary>
    ///科长审核时间
    /// </summary>
    public System.DateTime SC_AUDIT_TIME { get; set; }
        
    /// <summary>
    ///科长审核意见
    /// </summary>
    public string SC_OPINION { get; set; }
        
    /// <summary>
    ///审核部长
    /// </summary>
    public string MST_AUDITOR { get; set; }
        
    /// <summary>
    ///部长审核时间
    /// </summary>
    public System.DateTime MST_AUDIT_TIME { get; set; }
        
    /// <summary>
    ///部长审核意见
    /// </summary>
    public string MST_OPINION { get; set; }
        
    /// <summary>
    ///状态
    /// </summary>
    public string STATUS { get; set; }
        
    /// <summary>
    ///备注
    /// </summary>
    public string REMARK { get; set; }
        
    /// <summary>
    ///排列顺序
    /// </summary>
    public decimal ORDER_NO { get; set; }
        
    /// <summary>
    ///可用标记
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
    ///TEMPLATE_VERSION_NO
    /// </summary>
    public string TEMPLATE_VERSION_NO { get; set; }
        
    /// <summary>
    ///BI指标
    /// </summary>
    public string BI { get; set; }
    }
}
