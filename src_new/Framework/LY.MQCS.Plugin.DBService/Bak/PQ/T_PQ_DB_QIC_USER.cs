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
    ///QIC用户表
    /// </summary>
    public partial class T_PQ_DB_QIC_USER
    {
        
    /// <summary>
    ///QIC流程用户ID
    /// </summary>
    public string QIC_USER_ID { get; set; }
        
    /// <summary>
    ///用户登录名
    /// </summary>
    public string USERNAME_EN { get; set; }
        
    /// <summary>
    ///用户中文名
    /// </summary>
    public string USERNAME_CN { get; set; }
        
    /// <summary>
    ///邮箱
    /// </summary>
    public string EMAIL { get; set; }
        
    /// <summary>
    ///部门描述
    /// </summary>
    public string DEPT_DESC { get; set; }
        
    /// <summary>
    ///职位描述
    /// </summary>
    public string POSITION_DESC { get; set; }
        
    /// <summary>
    ///EAP组织ID（只参考）
    /// </summary>
    public string ORG_ID { get; set; }
        
    /// <summary>
    ///EAP组织编码（只参考）
    /// </summary>
    public string ORG_CODE { get; set; }
        
    /// <summary>
    ///业务提供用户清单顺序号
    /// </summary>
    public decimal SEQ { get; set; }
        
    /// <summary>
    ///EAP用户组ID
    /// </summary>
    public string USER_GROUP_ID { get; set; }
        
    /// <summary>
    ///QIC流程专用科室ID
    /// </summary>
    public string SECTION_ID { get; set; }
        
    /// <summary>
    ///QIC流程专用部门ID
    /// </summary>
    public string DEPT_ID { get; set; }
        
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