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
    ///PT后台日志表
    /// </summary>
    public partial class T_PQ_BU_PTLOG_APPLICATION
    {
        
    /// <summary>
    ///PT后台日志ID
    /// </summary>
    public string PTLOG_APPLICATION_ID { get; set; }
        
    /// <summary>
    ///功能名称
    /// </summary>
    public string FUNC_NAME { get; set; }
        
    /// <summary>
    ///执行步骤
    /// </summary>
    public string EXECUTE_STEP { get; set; }
        
    /// <summary>
    ///日志信息
    /// </summary>
    public string LOG_INFO { get; set; }
        
    /// <summary>
    ///记录时间
    /// </summary>
    public System.DateTime RECORD_TIME { get; set; }
        
    /// <summary>
    ///记录人 接口解析、FTP下载、CSV解析、sys，sys是默认记录人
    /// </summary>
    public string RECORDOR { get; set; }
        
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
