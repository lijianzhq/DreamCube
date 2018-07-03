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
    ///测量项目模板主表
    /// </summary>
    public partial class T_PQ_BU_MEAS_MOLD_M
    {
        
    /// <summary>
    ///测量项目模板主表ID
    /// </summary>
    public string MEAS_MOLD_M_ID { get; set; }
        
    /// <summary>
    ///模板名称
    /// </summary>
    public string MOLD_NAME { get; set; }
        
    /// <summary>
    ///测量类型
    /// </summary>
    public string MEASURE_TYPE { get; set; }
        
    /// <summary>
    ///部品名称
    /// </summary>
    public string PART_NAME { get; set; }
        
    /// <summary>
    ///工序编码
    /// </summary>
    public string PROCESS_CODE { get; set; }
        
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
        
    /// <summary>
    ///测量部位
    /// </summary>
    public string MEASURE_PART { get; set; }
        
    /// <summary>
    ///部品编码
    /// </summary>
    public string PART_CODE { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
    }
}