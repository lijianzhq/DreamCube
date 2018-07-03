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
    ///原材料供货信息
    /// </summary>
    public partial class T_PQ_BU_RAW_MAT_SUP
    {
        
    /// <summary>
    ///原材料供货信息ID
    /// </summary>
    public string RAW_MAT_SUP_ID { get; set; }
        
    /// <summary>
    ///原材料编码
    /// </summary>
    public string RAW_MAT_CODE { get; set; }
        
    /// <summary>
    ///购入批次号
    /// </summary>
    public string PUR_BATCH_CODE { get; set; }
        
    /// <summary>
    ///订单号
    /// </summary>
    public string ORDER_CODE { get; set; }
        
    /// <summary>
    ///订货人
    /// </summary>
    public string ORDER_MAN { get; set; }
        
    /// <summary>
    ///订货时间
    /// </summary>
    public System.DateTime ORDER_DATE { get; set; }
        
    /// <summary>
    ///物流编号
    /// </summary>
    public string LOGI_CODE { get; set; }
        
    /// <summary>
    ///物流公司
    /// </summary>
    public string LOGI_COMP { get; set; }
        
    /// <summary>
    ///仓库管理员
    /// </summary>
    public string WAREHOUSE_MAN { get; set; }
        
    /// <summary>
    ///仓库编码
    /// </summary>
    public string WAREHOUSE_CODE { get; set; }
        
    /// <summary>
    ///检测人
    /// </summary>
    public string CHECK_MAN { get; set; }
        
    /// <summary>
    ///原材料供货检测结果
    /// </summary>
    public string SUP_CHECK_RESULT { get; set; }
        
    /// <summary>
    ///检测地点
    /// </summary>
    public string CHECK_POS { get; set; }
        
    /// <summary>
    ///问题描述
    /// </summary>
    public string PROBLEM_DESC { get; set; }
        
    /// <summary>
    ///入库人员
    /// </summary>
    public string IN_STORE_MAN { get; set; }
        
    /// <summary>
    ///入库日期
    /// </summary>
    public System.DateTime IN_STORE_DATE { get; set; }
        
    /// <summary>
    ///入库地点
    /// </summary>
    public string IN_STORE_POS { get; set; }
        
    /// <summary>
    ///出库人员
    /// </summary>
    public string OUT_STORE_MAN { get; set; }
        
    /// <summary>
    ///出库日期
    /// </summary>
    public System.DateTime OUT_STORE_DATE { get; set; }
        
    /// <summary>
    ///出库地点
    /// </summary>
    public string OUT_STORE_POS { get; set; }
        
    /// <summary>
    ///原材料需求车间
    /// </summary>
    public string RAW_MAT_WS { get; set; }
        
    /// <summary>
    ///原材料供货状态
    /// </summary>
    public string RAW_MAT_SUP_STATUS { get; set; }
        
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
    }
}