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
    ///重保件信息表
    /// </summary>
    public partial class T_DB_DB_IP_INFO
    {
        
    /// <summary>
    ///重保件信息表ID
    /// </summary>
    public string IP_INFO_ID { get; set; }
        
    /// <summary>
    ///VIN码
    /// </summary>
    public string VIN { get; set; }
        
    /// <summary>
    ///重保件编码
    /// </summary>
    public string IP_CODE { get; set; }
        
    /// <summary>
    ///零件编码
    /// </summary>
    public string PART_NO { get; set; }
        
    /// <summary>
    ///零件号对应序号 如重保件清单中对应的重保件使用数是2;会拆出1;2两行
    /// </summary>
    public string PART_CODE_ORDER { get; set; }
        
    /// <summary>
    ///数量
    /// </summary>
    public decimal QTY { get; set; }
        
    /// <summary>
    ///零件名称
    /// </summary>
    public string PART_NAME { get; set; }
        
    /// <summary>
    ///录入日期
    /// </summary>
    public string ENTRY_DATE { get; set; }
        
    /// <summary>
    ///地点编码
    /// </summary>
    public string PLACE_CODE { get; set; }
        
    /// <summary>
    ///入库日期
    /// </summary>
    public string IN_STORE_DATE { get; set; }
        
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
    ///系统模块
    /// </summary>
    public string SYS_MODULE { get; set; }
        
    /// <summary>
    ///录入人员
    /// </summary>
    public string ENTRY_CLERK { get; set; }
        
    /// <summary>
    ///入库人员
    /// </summary>
    public string IN_STORE_MAN { get; set; }
        
    /// <summary>
    ///序号
    /// </summary>
    public int SEQ_NUM { get; set; }
        
    /// <summary>
    ///供应商名称
    /// </summary>
    public string SUPPLIER_NAME { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///轮胎均匀性状态 默认为0 0 待处理 1 处理成功 2 处理失败 3 处理中
    /// </summary>
    public string TIRE_UNIF_STATUS { get; set; }
    }
}
