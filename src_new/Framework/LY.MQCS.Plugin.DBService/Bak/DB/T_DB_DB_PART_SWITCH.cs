//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LY.MQCS.Plugin.DBService.Bak.DB
{
    using System;
    using System.Collections.Generic;
    
    
    /// <summary>
    ///零件切换信息业务表
    /// </summary>
    public partial class T_DB_DB_PART_SWITCH
    {
        
    /// <summary>
    ///零件切换信息ID
    /// </summary>
    public string PART_SWITCH { get; set; }
        
    /// <summary>
    ///设通号
    /// </summary>
    public string REQ_COMM_NO { get; set; }
        
    /// <summary>
    ///设通发布日期
    /// </summary>
    public string REQ_COMM_DIS_DATE { get; set; }
        
    /// <summary>
    ///工厂代码
    /// </summary>
    public string FAC_CODE { get; set; }
        
    /// <summary>
    ///车系编码
    /// </summary>
    public string CAR_SERIES_CODE { get; set; }
        
    /// <summary>
    ///新零件编号
    /// </summary>
    public string PART_NO_NEW { get; set; }
        
    /// <summary>
    ///旧零件编号
    /// </summary>
    public string PART_NO_OLD { get; set; }
        
    /// <summary>
    ///零件实际切换日期
    /// </summary>
    public string PART_FACT_SWITCH_DATE { get; set; }
        
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
    ///零件计划切换日期
    /// </summary>
    public string PART_PLAN_CHAN_DATE { get; set; }
        
    /// <summary>
    ///工程
    /// </summary>
    public string ENGINEERING_TYPE { get; set; }
        
    /// <summary>
    ///执行设通类型 (0:新车，1:量产)
    /// </summary>
    public string EXEC_DN_TYPE { get; set; }
    }
}