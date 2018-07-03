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
    ///品质指标表
    /// </summary>
    public partial class T_PQ_BU_QUALITY_INDEX
    {
        
    /// <summary>
    ///品质指标表ID
    /// </summary>
    public string QUALITY_INDEX_ID { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///车型编码 枚举:B12G;L12F;L12M
    /// </summary>
    public string CAR_TYPE_CODE { get; set; }
        
    /// <summary>
    ///日期
    /// </summary>
    public System.DateTime DATE_VAL { get; set; }
        
    /// <summary>
    ///指标类型 枚举:品质指标;监查指标
    /// </summary>
    public string INDEX_TYPE { get; set; }
        
    /// <summary>
    ///工程别 枚举:焊装二科;涂装二科;总装二科;整车
    /// </summary>
    public string PROJ_TYPE { get; set; }
        
    /// <summary>
    ///管理项目
    /// </summary>
    public string MANAGE_PROJ { get; set; }
        
    /// <summary>
    ///当日实绩
    /// </summary>
    public decimal DAY_PERFORMANCE { get; set; }
        
    /// <summary>
    ///当月实绩
    /// </summary>
    public decimal MON_PERFORMANCE { get; set; }
        
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