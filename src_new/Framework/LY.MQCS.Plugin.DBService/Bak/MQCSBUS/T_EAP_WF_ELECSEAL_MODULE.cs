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
    ///电子签章配置
    /// </summary>
    public partial class T_EAP_WF_ELECSEAL_MODULE
    {
        
    /// <summary>
    ///编号
    /// </summary>
    public string SEALMODULE_ID { get; set; }
        
    /// <summary>
    ///步骤guid
    /// </summary>
    public string STEP_GUID { get; set; }
        
    /// <summary>
    ///0：否，1：是
    /// </summary>
    public decimal SHOWSEALUI { get; set; }
        
    /// <summary>
    ///0：否，1：是
    /// </summary>
    public decimal NEEDSEAL { get; set; }
        
    /// <summary>
    ///-1：不限制，0：私章，1：公章
    /// </summary>
    public decimal SEALTYPE { get; set; }
        
    /// <summary>
    ///更新时间
    /// </summary>
    public System.DateTime LAST_UPDATETIME { get; set; }
        
    /// <summary>
    ///动作类型
    /// </summary>
    public decimal ACTIONTYPE { get; set; }
        
    /// <summary>
    ///用于打印表单时显示签章
    /// </summary>
    public string SEALPOS { get; set; }
    }
}