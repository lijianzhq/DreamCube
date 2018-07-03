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
    ///返修车信息_发送邮件设置
    /// </summary>
    public partial class T_PQ_BU_FX_SENDEMAIL_SET
    {
        
    /// <summary>
    ///发送邮件设置ID
    /// </summary>
    public string FX_SENDEMAIL_SET_ID { get; set; }
        
    /// <summary>
    ///计划报交时间起(含延后)
    /// </summary>
    public System.DateTime SUBMITTEDTIME_BEGIN { get; set; }
        
    /// <summary>
    ///计划报交时间止(含延后)
    /// </summary>
    public System.DateTime SUBMITTEDTIME_END { get; set; }
        
    /// <summary>
    ///车系
    /// </summary>
    public string VEHICLES { get; set; }
        
    /// <summary>
    ///提醒人员(注：email地址)(多人请用分号;隔开)
    /// </summary>
    public string REMINDINGPERSION { get; set; }
        
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
    ///延迟几小时提醒
    /// </summary>
    public decimal DELAHOURSTOREMIND { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
    }
}