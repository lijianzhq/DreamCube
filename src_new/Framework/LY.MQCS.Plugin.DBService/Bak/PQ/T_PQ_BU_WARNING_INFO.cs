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
    ///自定义预警信息表
    /// </summary>
    public partial class T_PQ_BU_WARNING_INFO
    {
        
    /// <summary>
    ///自定义预警信息表ID
    /// </summary>
    public string WARNING_INFO_ID { get; set; }
        
    /// <summary>
    ///预警编码
    /// </summary>
    public string WARNING_CODE { get; set; }
        
    /// <summary>
    ///预警类别
    /// </summary>
    public string WARNING_TYPE { get; set; }
        
    /// <summary>
    ///预警范围类型
    /// </summary>
    public string WARNING_RANGE { get; set; }
        
    /// <summary>
    ///预警原因
    /// </summary>
    public string WARNING_REASON { get; set; }
        
    /// <summary>
    ///部品类别编码
    /// </summary>
    public string PART_CODE { get; set; }
        
    /// <summary>
    ///部品类别名称
    /// </summary>
    public string PART_NAME { get; set; }
        
    /// <summary>
    ///生产线代码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
        
    /// <summary>
    ///生产线名称
    /// </summary>
    public string PRO_LINE_NAME { get; set; }
        
    /// <summary>
    ///工序编码
    /// </summary>
    public string PROCESS_CODE { get; set; }
        
    /// <summary>
    ///工序名称
    /// </summary>
    public string PROCESS_NAME { get; set; }
        
    /// <summary>
    ///预警时间
    /// </summary>
    public System.DateTime WARNING_TIME { get; set; }
        
    /// <summary>
    ///二维码
    /// </summary>
    public string QR_CODE { get; set; }
        
    /// <summary>
    ///处理方式 0：取消预警 1：状态更改
    /// </summary>
    public byte DEAL_TYPE { get; set; }
        
    /// <summary>
    ///预警状态 0：未处理 1：已处理
    /// </summary>
    public byte WARNING_STATUS { get; set; }
        
    /// <summary>
    ///部品状态 0：不修改状态 1：待处理 2：NG
    /// </summary>
    public byte PART_STATUS { get; set; }
        
    /// <summary>
    ///设备号
    /// </summary>
    public string EQUIP_CODE { get; set; }
        
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
    ///批次号
    /// </summary>
    public string BATCH_CODE { get; set; }
        
    /// <summary>
    ///实绩值
    /// </summary>
    public string ACTUAL_VAL { get; set; }
        
    /// <summary>
    ///处理说明
    /// </summary>
    public string DEAL_REMARK { get; set; }
        
    /// <summary>
    ///流水号
    /// </summary>
    public string FLOW_NO { get; set; }
        
    /// <summary>
    ///变更备注
    /// </summary>
    public string CHAN_REMARK { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///报文文件名
    /// </summary>
    public string IF_FILE_NAME { get; set; }
        
    /// <summary>
    ///处理人
    /// </summary>
    public string DEAL_PERSON { get; set; }
        
    /// <summary>
    ///规则极限值
    /// </summary>
    public string MAX_VAL { get; set; }
        
    /// <summary>
    ///测量字段
    /// </summary>
    public string MEASURE_CODE { get; set; }
    }
}