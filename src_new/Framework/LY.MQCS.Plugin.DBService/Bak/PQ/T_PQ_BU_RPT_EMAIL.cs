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
    ///报表邮件发送表
    /// </summary>
    public partial class T_PQ_BU_RPT_EMAIL
    {
        
    /// <summary>
    ///报表邮件发送ID
    /// </summary>
    public string RPT_EMAIL_ID { get; set; }
        
    /// <summary>
    ///报表收件人
    /// </summary>
    public string RPT_RECEIVER_MAIL { get; set; }
        
    /// <summary>
    ///报表URL参数
    /// </summary>
    public string RPT_URL_PARAM { get; set; }
        
    /// <summary>
    ///报表条件参数描述
    /// </summary>
    public string RPT_PARAM_DES { get; set; }
        
    /// <summary>
    ///报表邮件附件文件名
    /// </summary>
    public string RPT_FILENAME { get; set; }
        
    /// <summary>
    ///报表邮件标题
    /// </summary>
    public string RPT_TITLE { get; set; }
        
    /// <summary>
    ///报表邮件正文内容
    /// </summary>
    public string RPT_MAIN_BODY { get; set; }
        
    /// <summary>
    ///报表邮件日期
    /// </summary>
    public System.DateTime RPT_DATE { get; set; }
        
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
    ///邮件发送分组
    /// </summary>
    public string RPT_MAIL_GROUP { get; set; }
    }
}
