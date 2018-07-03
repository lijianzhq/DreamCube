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
    ///IQS对策答复单
    /// </summary>
    public partial class T_IQS_BU_TPR_REPLY
    {
        
    /// <summary>
    ///IQS对策答复单ID
    /// </summary>
    public string IQS_TPR_REPLY_ID { get; set; }
        
    /// <summary>
    ///发行编号
    /// </summary>
    public string BILL_NO { get; set; }
        
    /// <summary>
    ///分发人
    /// </summary>
    public string DISTRI_MAN { get; set; }
        
    /// <summary>
    ///分发日期
    /// </summary>
    public System.DateTime DISPENSE_DATE { get; set; }
        
    /// <summary>
    ///担当1
    /// </summary>
    public string DUTY_MAN1 { get; set; }
        
    /// <summary>
    ///担当2
    /// </summary>
    public string DUTY_MAN2 { get; set; }
        
    /// <summary>
    ///担当3
    /// </summary>
    public string DUTY_MAN3 { get; set; }
        
    /// <summary>
    ///状态
    /// </summary>
    public string STATUS { get; set; }
        
    /// <summary>
    ///备注
    /// </summary>
    public string REMARK { get; set; }
        
    /// <summary>
    ///提交人
    /// </summary>
    public string SUBMITOR { get; set; }
        
    /// <summary>
    ///提交时间
    /// </summary>
    public System.DateTime SUBMIT_TIME { get; set; }
        
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
    ///审核担当（TCS)
    /// </summary>
    public string TCS_AUDITOR { get; set; }
        
    /// <summary>
    ///担当审核时间（TCS)
    /// </summary>
    public System.DateTime TCS_AUDIT_TIME { get; set; }
        
    /// <summary>
    ///担当审核意见（TCS)
    /// </summary>
    public string TCS_OPINION { get; set; }
        
    /// <summary>
    ///审核科长（TCS)
    /// </summary>
    public string TCS_SC_AUDITOR { get; set; }
        
    /// <summary>
    ///科长审核时间（TCS）
    /// </summary>
    public System.DateTime TCS_SC_AUDIT_TIME { get; set; }
        
    /// <summary>
    ///科长审核意见（TCS）
    /// </summary>
    public string TCS_SC_OPINION { get; set; }
        
    /// <summary>
    ///审核部长（TCS）
    /// </summary>
    public string TCS_MST_AUDITOR { get; set; }
        
    /// <summary>
    ///部长审核时间（TCS）
    /// </summary>
    public System.DateTime TCS_MST_AUDIT_TIME { get; set; }
        
    /// <summary>
    ///部长审核意见（TCS）
    /// </summary>
    public string TCS_MST_OPINION { get; set; }
        
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
    ///修正后答复日期:由于案件答复日期，有时候会主动修改（因为节假日的原因）
    /// </summary>
    public System.DateTime MODIFY_ANSWER_TIME { get; set; }
        
    /// <summary>
    ///报告导入日期修正后
    /// </summary>
    public System.DateTime REPORT_IMP_DATE_MODIFY { get; set; }
        
    /// <summary>
    ///修改内容
    /// </summary>
    public string UPDATE_CONTENT { get; set; }
        
    /// <summary>
    ///修改原因
    /// </summary>
    public string UPDATE_REASON { get; set; }
        
    /// <summary>
    ///驳回节点状态
    /// </summary>
    public string REJE_NODE_STATE { get; set; }
    }
}
