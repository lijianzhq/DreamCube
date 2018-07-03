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
    ///判断责任部门信息表
    /// </summary>
    public partial class T_PQ_DB_DUTY_DEPT
    {
        
    /// <summary>
    ///责任部门ID
    /// </summary>
    public string DUTY_DEPT_ID { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///部门类别编码
    /// </summary>
    public string DEPT_TYPE_CODE { get; set; }
        
    /// <summary>
    ///部门类别名称
    /// </summary>
    public string DEPT_TYPE_NAME { get; set; }
        
    /// <summary>
    ///判断工程编码
    /// </summary>
    public string ENGINEERING_CODE { get; set; }
        
    /// <summary>
    ///判断工程名称
    /// </summary>
    public string ENGINEERING_NAME { get; set; }
        
    /// <summary>
    ///责任部门编码
    /// </summary>
    public string DUTY_DEPT_CODE { get; set; }
        
    /// <summary>
    ///责任部门名称
    /// </summary>
    public string DUTY_DEPT_NAME { get; set; }
        
    /// <summary>
    ///VARCHAR2
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
