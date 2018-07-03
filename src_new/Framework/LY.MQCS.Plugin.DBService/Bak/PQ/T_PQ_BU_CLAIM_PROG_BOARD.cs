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
    ///索赔进展看板表
    /// </summary>
    public partial class T_PQ_BU_CLAIM_PROG_BOARD
    {
        
    /// <summary>
    ///索赔进展看板ID
    /// </summary>
    public string CLAIM_PROG_BOARD_ID { get; set; }
        
    /// <summary>
    ///索赔月
    /// </summary>
    public string CLAIM_MON { get; set; }
        
    /// <summary>
    ///索赔信息发布标识
    /// </summary>
    public string CLAIM_INFO_RELE_FLAG { get; set; }
        
    /// <summary>
    ///检提状态
    /// </summary>
    public string CHECK_ADV_STATUS { get; set; }
        
    /// <summary>
    ///申诉标识
    /// </summary>
    public string APPEAL_FLAG { get; set; }
        
    /// <summary>
    ///索赔金额明细发布标识
    /// </summary>
    public string CLAIM_AMT_D_RELE_FLAG { get; set; }
        
    /// <summary>
    ///索赔明细确认标识
    /// </summary>
    public string CLAIM_D_COMF_FLAG { get; set; }
        
    /// <summary>
    ///索赔单位
    /// </summary>
    public string CLAIM_UNIT { get; set; }
        
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
    ///锁定状态 0-为未锁定;1-为已锁定;默认0
    /// </summary>
    public string LOCK_STATUS { get; set; }
        
    /// <summary>
    ///不合格品单数
    /// </summary>
    public int BILL_NUM { get; set; }
        
    /// <summary>
    ///日产发票寄出标识
    /// </summary>
    public string NISSAN_INVO_SEND_FLAG { get; set; }
        
    /// <summary>
    ///启辰发票寄出标识
    /// </summary>
    public string QC_INVO_SEND_FLAG { get; set; }
        
    /// <summary>
    ///日产发票寄送日期
    /// </summary>
    public System.DateTime NISSAN_INVO_SEND_DATE { get; set; }
        
    /// <summary>
    ///启辰发票寄送日期
    /// </summary>
    public System.DateTime QC_INVO_SEND_DATE { get; set; }
    }
}
