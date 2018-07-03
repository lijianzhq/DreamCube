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
    ///T_IFR_IQS_BU_DISPATCH_E3S
    /// </summary>
    public partial class T_IFR_IQS_BU_DISPATCH_E3S
    {
        
    /// <summary>
    ///ID
    /// </summary>
    public string ID { get; set; }
        
    /// <summary>
    ///发行编号
    /// </summary>
    public string BILL_NO { get; set; }
        
    /// <summary>
    ///部品编号
    /// </summary>
    public string PART_CODE { get; set; }
        
    /// <summary>
    ///实到数量
    /// </summary>
    public decimal REAL_QTY { get; set; }
        
    /// <summary>
    ///返回类型
    /// </summary>
    public string RETURN_TYPE { get; set; }
        
    /// <summary>
    ///运输公司
    /// </summary>
    public string POST_COMP { get; set; }
        
    /// <summary>
    ///邮寄单号
    /// </summary>
    public string POST_NO { get; set; }
        
    /// <summary>
    ///AS发出日
    /// </summary>
    public System.DateTime DATE_AS_SEND { get; set; }
        
    /// <summary>
    ///DLR发出日
    /// </summary>
    public System.DateTime DATE_DLR_SEND { get; set; }
        
    /// <summary>
    ///AS回复日
    /// </summary>
    public System.DateTime DATE_AS_ANSWER { get; set; }
        
    /// <summary>
    ///状态
    /// </summary>
    public string STATUS { get; set; }
        
    /// <summary>
    ///处理时间
    /// </summary>
    public System.DateTime HANDLED_TIME { get; set; }
        
    /// <summary>
    ///处理状态标识
    /// </summary>
    public string HANDLE_FLAG { get; set; }
        
    /// <summary>
    ///创建人
    /// </summary>
    public string CREATOR { get; set; }
        
    /// <summary>
    ///创建时间
    /// </summary>
    public System.DateTime CREATE_TIME { get; set; }
        
    /// <summary>
    ///修改人
    /// </summary>
    public string MODIFIER { get; set; }
        
    /// <summary>
    ///最后更新时间
    /// </summary>
    public System.DateTime LAST_UPDATED_TIME { get; set; }
        
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
    }
}
