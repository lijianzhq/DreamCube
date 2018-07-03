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
    ///T_EAP_FORM_STEP_EXECHISTORY
    /// </summary>
    public partial class T_EAP_FORM_STEP_EXECHISTORY
    {
        
    /// <summary>
    ///表ID
    /// </summary>
    public string ID { get; set; }
        
    /// <summary>
    ///执行了的规则
    /// </summary>
    public string RULE_ID { get; set; }
        
    /// <summary>
    ///执行的步骤ID
    /// </summary>
    public string STEP_ID { get; set; }
        
    /// <summary>
    ///执行步骤对应的参数
    /// </summary>
    public byte[] STEP_PARAMS { get; set; }
        
    /// <summary>
    ///执行内容
    /// </summary>
    public string STEP_CONTENT { get; set; }
        
    /// <summary>
    ///执行次数
    /// </summary>
    public decimal COUNT { get; set; }
        
    /// <summary>
    ///执行时间
    /// </summary>
    public decimal EXECUTE_TIME { get; set; }
        
    /// <summary>
    ///表单设置规则的ID
    /// </summary>
    public string RULE_FORMRULE_ID { get; set; }
        
    /// <summary>
    ///步骤类型
    /// </summary>
    public string STEP_TYPE { get; set; }
        
    /// <summary>
    ///步骤执行结果
    /// </summary>
    public string STEP_RESULT { get; set; }
        
    /// <summary>
    ///步骤执行顺序
    /// </summary>
    public decimal STEP_EXEC_ORDER { get; set; }
        
    /// <summary>
    ///创建时间
    /// </summary>
    public System.DateTime CREATED_TIME { get; set; }
    }
}
