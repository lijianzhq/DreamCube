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
    ///待办运行表
    /// </summary>
    public partial class T_EAP_WF_PENDING
    {
        
    /// <summary>
    ///编号
    /// </summary>
    public string PENDING_ID { get; set; }
        
    /// <summary>
    ///待办标题
    /// </summary>
    public string PENDING_TITLE { get; set; }
        
    /// <summary>
    ///待办来源用户
    /// </summary>
    public string FROM_USER { get; set; }
        
    /// <summary>
    ///待办目标用户
    /// </summary>
    public string TO_USER { get; set; }
        
    /// <summary>
    ///备注
    /// </summary>
    public string DESCRIPTION { get; set; }
        
    /// <summary>
    ///节点编号
    /// </summary>
    public string NODE_GUID { get; set; }
        
    /// <summary>
    ///处理人ID
    /// </summary>
    public string AUTOR_GUID { get; set; }
        
    /// <summary>
    ///处理人名称
    /// </summary>
    public string AUTOR_NAME { get; set; }
        
    /// <summary>
    ///待办用户
    /// </summary>
    public string PEND_AUTOR { get; set; }
        
    /// <summary>
    ///待办类型
    /// </summary>
    public decimal PENDING_TYPE { get; set; }
        
    /// <summary>
    ///待办状态
    /// </summary>
    public decimal PENDING_STATUS { get; set; }
        
    /// <summary>
    ///处理时间
    /// </summary>
    public System.DateTime PROCESSING_TIME { get; set; }
        
    /// <summary>
    ///最后更新时间
    /// </summary>
    public System.DateTime LAST_UPDATED_DATE { get; set; }
        
    /// <summary>
    ///创建时间
    /// </summary>
    public System.DateTime CREATE_TIME { get; set; }
        
    /// <summary>
    ///流程编号
    /// </summary>
    public string PROCESS_GUID { get; set; }
        
    /// <summary>
    ///1 未删除  2  已删除
    /// </summary>
    public decimal DELETED { get; set; }
        
    /// <summary>
    ///优先级
    /// </summary>
    public decimal PRIORITY { get; set; }
        
    /// <summary>
    ///节点类型
    /// </summary>
    public decimal NODE_FROM { get; set; }
        
    /// <summary>
    ///待办类型名称
    /// </summary>
    public string PENDING_TYPENAME { get; set; }
        
    /// <summary>
    ///路径编号
    /// </summary>
    public string STEP_PATH_GUID { get; set; }
        
    /// <summary>
    ///激活时间
    /// </summary>
    public System.DateTime ACTIVE_DATE { get; set; }
    }
}
