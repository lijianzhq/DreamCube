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
    ///变速箱装配品质部品信息表
    /// </summary>
    public partial class T_PQ_BU_GEAR_ASSE_QA_PART
    {
        
    /// <summary>
    ///变速箱装配品质部品信息ID
    /// </summary>
    public string GEAR_ASSE_QA_PART_ID { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///工厂名称
    /// </summary>
    public string FACTORY_NAME { get; set; }
        
    /// <summary>
    ///生产线编码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
        
    /// <summary>
    ///机种
    /// </summary>
    public string MAC_TYPE { get; set; }
        
    /// <summary>
    ///变速箱号
    /// </summary>
    public string GEAR_BOX { get; set; }
        
    /// <summary>
    ///部品类别
    /// </summary>
    public string PART_SORT { get; set; }
        
    /// <summary>
    ///部品二维码
    /// </summary>
    public string PART_2D_CODE { get; set; }
        
    /// <summary>
    ///接收时间
    /// </summary>
    public System.DateTime RECEIVE_TIME { get; set; }
        
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
    ///有效性 1-有效，0-无效
    /// </summary>
    public string PART_EFFECT { get; set; }
        
    /// <summary>
    ///部品更换人
    /// </summary>
    public string PART_CHANGE_MAN { get; set; }
        
    /// <summary>
    ///部品更换时间
    /// </summary>
    public System.DateTime PART_CHANGE_TIME { get; set; }
        
    /// <summary>
    ///部品更换备注
    /// </summary>
    public string PART_CHANGE_REMARK { get; set; }
    
        public virtual T_PQ_BU_GEAR_ASSE_QA T_PQ_BU_GEAR_ASSE_QA { get; set; }
    }
}