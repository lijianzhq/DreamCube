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
    ///用户流程代理设置表
    /// </summary>
    public partial class T_EAP_WF_PROCESSAGENT
    {
        
    /// <summary>
    ///用户编号
    /// </summary>
    public string USER_ID { get; set; }
        
    /// <summary>
    ///代理人编号
    /// </summary>
    public string AGENTUSER_ID { get; set; }
        
    /// <summary>
    ///流程模版编号
    /// </summary>
    public string PROCESS_GUID { get; set; }
        
    /// <summary>
    ///0：否，1：是
    /// </summary>
    public bool ISFILTER { get; set; }
        
    /// <summary>
    ///激活时间
    /// </summary>
    public System.DateTime ACTIVEDATE { get; set; }
        
    /// <summary>
    ///结束时间
    /// </summary>
    public System.DateTime DISABLEDATE { get; set; }
        
    /// <summary>
    ///0：否，1：是
    /// </summary>
    public bool ISACTIVE { get; set; }
        
    /// <summary>
    ///创建人
    /// </summary>
    public string CREATED_BY { get; set; }
        
    /// <summary>
    ///创建时间
    /// </summary>
    public System.DateTime CREATED_TIME { get; set; }
        
    /// <summary>
    ///AGENT_ID
    /// </summary>
    public string AGENT_ID { get; set; }
    }
}
