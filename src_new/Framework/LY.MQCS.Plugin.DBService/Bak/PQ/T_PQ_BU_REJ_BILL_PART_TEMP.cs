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
    ///不合格品单牵连件临时表
    /// </summary>
    public partial class T_PQ_BU_REJ_BILL_PART_TEMP
    {
        
    /// <summary>
    ///不合格品单牵连件临时表ID
    /// </summary>
    public string REJ_BILL_PART_TEMP_ID { get; set; }
        
    /// <summary>
    ///系统不合格品单ID
    /// </summary>
    public string SYS_REJ_BILL_ID { get; set; }
        
    /// <summary>
    ///不合格品单ID
    /// </summary>
    public string REJ_BILL_ID { get; set; }
        
    /// <summary>
    ///零件编码
    /// </summary>
    public string PART_NO { get; set; }
        
    /// <summary>
    ///零件名称
    /// </summary>
    public string PART_NAME { get; set; }
        
    /// <summary>
    ///零件数量
    /// </summary>
    public int PART_QTY { get; set; }
        
    /// <summary>
    ///是否主凶件
    /// </summary>
    public string IS_PFP { get; set; }
        
    /// <summary>
    ///是否索赔主凶件
    /// </summary>
    public string IS_INDEM_PFP { get; set; }
        
    /// <summary>
    ///供应商编码
    /// </summary>
    public string SUPPLIER_CODE { get; set; }
        
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
