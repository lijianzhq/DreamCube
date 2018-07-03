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
    ///RS项目系统附件表
    /// </summary>
    public partial class T_PQ_BU_RSATTACHMENT
    {
        
    /// <summary>
    ///附件表ID
    /// </summary>
    public string RSATTACHMENT_ID { get; set; }
        
    /// <summary>
    ///对应RS项目系统主表ID
    /// </summary>
    public string RSPROJECT_INFO_ID { get; set; }
        
    /// <summary>
    ///对应附件类型
    /// </summary>
    public string FILE_BELONGED { get; set; }
        
    /// <summary>
    ///文件路径
    /// </summary>
    public string FILE_PATH { get; set; }
        
    /// <summary>
    ///文件类型
    /// </summary>
    public string FILE_TYPE { get; set; }
        
    /// <summary>
    ///文件名称
    /// </summary>
    public string FILE_NAME { get; set; }
        
    /// <summary>
    ///文件大小
    /// </summary>
    public decimal FILE_SIZE { get; set; }
        
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
