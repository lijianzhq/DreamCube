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
    ///QIC质量异常处理单追溯确认情况明细表
    /// </summary>
    public partial class T_PQ_BU_QIC_EXCE_D_HCFM
    {
        
    /// <summary>
    ///QIC质量异常处理单主表ID
    /// </summary>
    public string QIC_EXCE_M_ID { get; set; }
        
    /// <summary>
    ///QIC质量异常处理单追溯确认情况明细ID
    /// </summary>
    public string QIC_EXCE_D_HCFM_ID { get; set; }
        
    /// <summary>
    ///QIC单号
    /// </summary>
    public string QIC_CODE { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///QIC单据类型
    /// </summary>
    public string QIC_BILL_TYPE { get; set; }
        
    /// <summary>
    ///工程
    /// </summary>
    public string CRAFT { get; set; }
        
    /// <summary>
    ///结果数
    /// </summary>
    public string CONFIRM_RESULT_QTY { get; set; }
        
    /// <summary>
    ///确认人
    /// </summary>
    public string CONFIRMER { get; set; }
        
    /// <summary>
    ///备注
    /// </summary>
    public string CONFIRM_DESC { get; set; }
        
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
    ///不良数
    /// </summary>
    public string CONFIRM_BAD_QTY { get; set; }
    
        public virtual T_PQ_BU_QIC_EXCE_M T_PQ_BU_QIC_EXCE_M { get; set; }
    }
}
