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
    ///不合格品判定申请缺陷表
    /// </summary>
    public partial class T_BU_PQ_REJ_JG_APP_DEFECT
    {
        
    /// <summary>
    ///不合格品判定申请缺陷表ID
    /// </summary>
    public string REJ_JG_APP_DEFECT_ID { get; set; }
        
    /// <summary>
    ///不合格品单号
    /// </summary>
    public string REJ_BILL_CODE { get; set; }
        
    /// <summary>
    ///缺陷问题信息表ID
    /// </summary>
    public string DEFECT_QUES_ID { get; set; }
        
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
        
    /// <summary>
    ///最后操作时间（用于历史表对应上）
    /// </summary>
    public System.DateTime LAST_OPT_TIME { get; set; }
    }
}
