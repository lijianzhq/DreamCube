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
    ///T_Eap_Sys_User
    /// </summary>
    public partial class T_EAP_SYS_USER
    {
        
    /// <summary>
    ///用户ID
    /// </summary>
    public string USER_ID { get; set; }
        
    /// <summary>
    ///员工ID
    /// </summary>
    public string EMP_ID { get; set; }
        
    /// <summary>
    ///登录名称
    /// </summary>
    public string USER_NAME { get; set; }
        
    /// <summary>
    ///密码
    /// </summary>
    public string PASSWORD { get; set; }
        
    /// <summary>
    ///用户类型
    /// </summary>
    public decimal USER_TYPE { get; set; }
        
    /// <summary>
    ///说明
    /// </summary>
    public string USER_DESC { get; set; }
        
    /// <summary>
    ///用户所属组织
    /// </summary>
    public string ORG_ID { get; set; }
        
    /// <summary>
    ///生效日期
    /// </summary>
    public System.DateTime ACTIVE_DATE { get; set; }
        
    /// <summary>
    ///失效日期
    /// </summary>
    public System.DateTime DISABLE_DATE { get; set; }
        
    /// <summary>
    ///是否有效
    /// </summary>
    public decimal ENABLED { get; set; }
        
    /// <summary>
    ///是否内置
    /// </summary>
    public decimal BUILT_IN { get; set; }
        
    /// <summary>
    ///创建人
    /// </summary>
    public string CREATED_BY { get; set; }
        
    /// <summary>
    ///创建时间
    /// </summary>
    public System.DateTime CREATED_TIME { get; set; }
        
    /// <summary>
    ///最后修改人
    /// </summary>
    public string LAST_UPDATED_BY { get; set; }
        
    /// <summary>
    ///最后修改时间
    /// </summary>
    public System.DateTime LAST_UPDATED_TIME { get; set; }
        
    /// <summary>
    ///USER_NICKNAME
    /// </summary>
    public string USER_NICKNAME { get; set; }
        
    /// <summary>
    ///LYCCUSER_ID
    /// </summary>
    public string LYCCUSER_ID { get; set; }
        
    /// <summary>
    ///LYCC_NO
    /// </summary>
    public string LYCC_NO { get; set; }
        
    /// <summary>
    ///用户名备份（域切换新增）
    /// </summary>
    public string USER_NAME_BAK { get; set; }
    }
}