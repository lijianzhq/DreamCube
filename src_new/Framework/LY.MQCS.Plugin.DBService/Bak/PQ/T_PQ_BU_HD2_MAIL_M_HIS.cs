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
    ///HD2邮件表历史表
    /// </summary>
    public partial class T_PQ_BU_HD2_MAIL_M_HIS
    {
        
    /// <summary>
    ///HD2邮件表历史ID
    /// </summary>
    public string HD2_MAIL_M_HIS_ID { get; set; }
        
    /// <summary>
    ///HD2邮件ID
    /// </summary>
    public string HD2_MAIL_M_ID { get; set; }
        
    /// <summary>
    ///邮件标题
    /// </summary>
    public string EMAIL_SUBJECT { get; set; }
        
    /// <summary>
    ///邮件内容
    /// </summary>
    public string EMAIL_BODY { get; set; }
        
    /// <summary>
    ///邮件接收人名称，用“;”号分割
    /// </summary>
    public string MAIL_RECEIVER_NAME { get; set; }
        
    /// <summary>
    ///邮件接收人地址，用“:”号分割
    /// </summary>
    public string MAIL_ADDRESS { get; set; }
        
    /// <summary>
    ///附件路径 附件的相对路径
    /// </summary>
    public string FILE_PATH { get; set; }
        
    /// <summary>
    ///发送标识，0：未发送，1：发送成功，2：发送失败，发送失败的数据不为重新发送
    /// </summary>
    public string SEND_FLAG { get; set; }
        
    /// <summary>
    ///发送时间，为空代表实时发送
    /// </summary>
    public System.DateTime SEND_TIME { get; set; }
        
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