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
    ///不合格品判定申请零件箱表
    /// </summary>
    public partial class T_PQ_BU_REJ_JUD_PART_BOX
    {
        
    /// <summary>
    ///不合格品判定申请零件箱表ID
    /// </summary>
    public string REJ_JUD_PART_BOX_ID { get; set; }
        
    /// <summary>
    ///不合格品单号（新单号）判定生成
    /// </summary>
    public string REJ_BILL_CODE { get; set; }
        
    /// <summary>
    ///零件箱标签号
    /// </summary>
    public string PART_BOX_CODE { get; set; }
        
    /// <summary>
    ///申请数
    /// </summary>
    public decimal APPL_QTY { get; set; }
        
    /// <summary>
    ///ng数
    /// </summary>
    public decimal NG_QTY { get; set; }
        
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
    ///不合格品单号（原单号）来源申请
    /// </summary>
    public string REJ_BILL_CODE_APPL { get; set; }
        
    /// <summary>
    ///责任单位编码
    /// </summary>
    public string DUTY_COMP_CODE { get; set; }
        
    /// <summary>
    ///责任单位名称
    /// </summary>
    public string DUTY_COMP_NAME { get; set; }
        
    /// <summary>
    ///承担比例
    /// </summary>
    public decimal BEAR_RATIO { get; set; }
        
    /// <summary>
    ///承担总金额
    /// </summary>
    public decimal BEAR_AMT_TOT { get; set; }
        
    /// <summary>
    ///责任单位类别
    /// </summary>
    public string DUTY_COMP_TYPE { get; set; }
        
    /// <summary>
    ///索赔类型
    /// </summary>
    public string CLAIM_TYPE { get; set; }
        
    /// <summary>
    ///责任科室CODE
    /// </summary>
    public string DUTY_SEC_CODE { get; set; }
        
    /// <summary>
    ///不良描述
    /// </summary>
    public string BAD_DESC { get; set; }
        
    /// <summary>
    ///当前状态（-1被驳回，0待判定，1箱子已判定，2汇总单已判定完成）
    /// </summary>
    public string CUR_STATUS { get; set; }
        
    /// <summary>
    ///判定人
    /// </summary>
    public string DECI_MAN { get; set; }
        
    /// <summary>
    ///判定时间
    /// </summary>
    public System.DateTime DECI_TIME { get; set; }
        
    /// <summary>
    ///报废类型（0工废，1料废）
    /// </summary>
    public string SCRAP_TYPE { get; set; }
        
    /// <summary>
    ///责任单位类别名称（冗余）
    /// </summary>
    public string DUTY_COMP_TYPE_NAME { get; set; }
        
    /// <summary>
    ///索赔类型名称（冗余）
    /// </summary>
    public string CLAIM_TYPE_NAME { get; set; }
        
    /// <summary>
    ///责任科室名称（冗余）
    /// </summary>
    public string DUTY_SEC_NAME { get; set; }
    }
}
