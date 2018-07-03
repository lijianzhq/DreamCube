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
    ///曲轴孔分级打号信息表
    /// </summary>
    public partial class T_PQ_BU_CS_HOLE_C
    {
        
    /// <summary>
    ///曲轴孔分级打号信息ID
    /// </summary>
    public string CS_HOLE_C_ID { get; set; }
        
    /// <summary>
    ///部品工序信息ID
    /// </summary>
    public string PART_PROCESS_ID { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///部品类别
    /// </summary>
    public string PART_SORT { get; set; }
        
    /// <summary>
    ///部品二维码
    /// </summary>
    public string PART_2D_CODE { get; set; }
        
    /// <summary>
    ///曲轴孔测量数据直径X
    /// </summary>
    public decimal DIA_CRANK_SEC_X { get; set; }
        
    /// <summary>
    ///曲轴孔测量数据直径Y
    /// </summary>
    public decimal DIA_CRANK_SEC_Y { get; set; }
        
    /// <summary>
    ///曲轴孔分级打号数据
    /// </summary>
    public string CS_HOLE_C_DATA { get; set; }
        
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
