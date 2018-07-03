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
    ///不良品数据表
    /// </summary>
    public partial class T_PQ_BU_BAD_DATA
    {
        
    /// <summary>
    ///不良品数据ID
    /// </summary>
    public string BAD_DATA_ID { get; set; }
        
    /// <summary>
    ///部品二维码
    /// </summary>
    public string PART_2D_CODE { get; set; }
        
    /// <summary>
    ///零件编码
    /// </summary>
    public string PART_NO { get; set; }
        
    /// <summary>
    ///生产线编码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
        
    /// <summary>
    ///工序编码
    /// </summary>
    public string PROCESS_CODE { get; set; }
        
    /// <summary>
    ///设备/位置
    /// </summary>
    public string EQUIP_POS { get; set; }
        
    /// <summary>
    ///判定/变更时间
    /// </summary>
    public System.DateTime DECI_CHAN_TIME { get; set; }
        
    /// <summary>
    ///责任部门
    /// </summary>
    public string DUTY_DEPT { get; set; }
        
    /// <summary>
    ///不良品数据来源
    /// </summary>
    public string BAD_PART_SOURCE { get; set; }
        
    /// <summary>
    ///变更批次
    /// </summary>
    public string CHANGE_BAT { get; set; }
        
    /// <summary>
    ///变更数量
    /// </summary>
    public int CHANGE_QTY { get; set; }
        
    /// <summary>
    ///部品变更状态
    /// </summary>
    public string PART_CHAN_TYPE { get; set; }
        
    /// <summary>
    ///状态变更/判定人
    /// </summary>
    public string STATUS_CHAN_DECI_MAN { get; set; }
        
    /// <summary>
    ///不良大类
    /// </summary>
    public string BAD_BIG_TYPE { get; set; }
        
    /// <summary>
    ///不良类型
    /// </summary>
    public string BAD_TYPE { get; set; }
        
    /// <summary>
    ///变更备注
    /// </summary>
    public string CHANGE_REMARK { get; set; }
        
    /// <summary>
    ///是否生效
    /// </summary>
    public string IS_EFFE { get; set; }
        
    /// <summary>
    ///审批人
    /// </summary>
    public string APPROVAL_MAN { get; set; }
        
    /// <summary>
    ///审批时间
    /// </summary>
    public System.DateTime APPROVAL_TIME { get; set; }
        
    /// <summary>
    ///审批意见
    /// </summary>
    public string APPROVAL_OPIN { get; set; }
        
    /// <summary>
    ///审批态度
    /// </summary>
    public string APPROVAL_ATTI { get; set; }
        
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
    ///机种
    /// </summary>
    public string MAC_TYPE { get; set; }
        
    /// <summary>
    ///工序加工时间
    /// </summary>
    public System.DateTime PROCESS_TIME { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///部品类别
    /// </summary>
    public string PART_TYPE { get; set; }
        
    /// <summary>
    ///工厂名称
    /// </summary>
    public string FACTORY_NAME { get; set; }
        
    /// <summary>
    ///不良部品类型 '1'发动机内置,'2'发动机外购,'3'变速箱部品,'4'发动机整机,'5'变速箱整机
    /// </summary>
    public string BAD_PART_TYPE { get; set; }
        
    /// <summary>
    ///逻辑工序
    /// </summary>
    public string LOGIC_PROCESS { get; set; }
        
    /// <summary>
    ///是否已处理 默认值0。0未处理；1已处理
    /// </summary>
    public string IS_DEAL { get; set; }
        
    /// <summary>
    ///待处理事项处置描述
    /// </summary>
    public string DEAL_REMARK { get; set; }
        
    /// <summary>
    ///关联履历ID 部品状态审核待处理事项关联履历ID
    /// </summary>
    public string RELATED_BAD_DATA_ID { get; set; }
    }
}