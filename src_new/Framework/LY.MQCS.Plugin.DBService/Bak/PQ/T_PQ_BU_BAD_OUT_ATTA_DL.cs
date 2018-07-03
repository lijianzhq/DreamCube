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
    ///不良流出管理附件下载历史数据表
    /// </summary>
    public partial class T_PQ_BU_BAD_OUT_ATTA_DL
    {
        
    /// <summary>
    ///不良流出管理附件下载历史数据表ID
    /// </summary>
    public string BAD_OUT_ATTA_DL_ID { get; set; }
        
    /// <summary>
    ///附件表记录的ID值
    /// </summary>
    public string ATTA_FILE_ID { get; set; }
        
    /// <summary>
    ///下载人（user_name）
    /// </summary>
    public string DOWNLOAD_MAN { get; set; }
        
    /// <summary>
    ///下载时间
    /// </summary>
    public System.DateTime DOWNLOAD_TIME { get; set; }
        
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
