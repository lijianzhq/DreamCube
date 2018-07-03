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
    ///测量项目实绩从表
    /// </summary>
    public partial class T_PQ_BU_MEAS_RESULT_D
    {
        
    /// <summary>
    ///测量项目实绩从表ID
    /// </summary>
    public string MEAS_RESULT_D_ID { get; set; }
        
    /// <summary>
    ///测量项目实绩主表ID
    /// </summary>
    public string MEAS_RESULT_M_ID { get; set; }
        
    /// <summary>
    ///测量项目
    /// </summary>
    public string MEASURE_ITEM { get; set; }
        
    /// <summary>
    ///测量点
    /// </summary>
    public string MEASURE_POINT { get; set; }
        
    /// <summary>
    ///基准
    /// </summary>
    public decimal BASE_VAL { get; set; }
        
    /// <summary>
    ///上差
    /// </summary>
    public decimal UPPER_DEVIATION { get; set; }
        
    /// <summary>
    ///下差
    /// </summary>
    public decimal LOWER_DEVIATION { get; set; }
        
    /// <summary>
    ///实测值（偏差值）
    /// </summary>
    public decimal ACTUAL_VAL { get; set; }
        
    /// <summary>
    ///测量结果
    /// </summary>
    public string MEASURE_RESULT { get; set; }
        
    /// <summary>
    ///单位
    /// </summary>
    public string UNIT { get; set; }
        
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
        
    /// <summary>
    ///项目序号
    /// </summary>
    public decimal ITEM_NO { get; set; }
        
    /// <summary>
    ///备注
    /// </summary>
    public string REMARK { get; set; }
    }
}
