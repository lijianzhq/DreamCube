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
    ///合同信息表
    /// </summary>
    public partial class T_PQ_BU_CONTRACT
    {
        
    /// <summary>
    ///合同ID
    /// </summary>
    public string CONTRACT_ID { get; set; }
        
    /// <summary>
    ///物料编码
    /// </summary>
    public string MATERIAL_CODE { get; set; }
        
    /// <summary>
    ///合同号
    /// </summary>
    public string CONTRACT_CODE { get; set; }
        
    /// <summary>
    ///期货月
    /// </summary>
    public string FUTURES_MON { get; set; }
        
    /// <summary>
    ///供应商编码
    /// </summary>
    public string SUPPLIER_CODE { get; set; }
        
    /// <summary>
    ///工厂ID
    /// </summary>
    public string FACTORY_ID { get; set; }
        
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
