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
    ///发动机装配批次部品信息表
    /// </summary>
    public partial class T_PQ_BU_E_ASSE_BAT_PART
    {
        
    /// <summary>
    ///发动机装配批次部品信息ID
    /// </summary>
    public string E_ASSE_BAT_PART_ID { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///发动机号
    /// </summary>
    public string ENGINE_NO { get; set; }
        
    /// <summary>
    ///部品类别
    /// </summary>
    public string PART_SORT { get; set; }
        
    /// <summary>
    ///部品批次号
    /// </summary>
    public string PART_NUM { get; set; }
        
    /// <summary>
    ///成品二维码
    /// </summary>
    public string PROD_2D_CODE { get; set; }
        
    /// <summary>
    ///外购部品状态
    /// </summary>
    public string OUTPART_STATUS { get; set; }
        
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
    ///工厂名称
    /// </summary>
    public string FACTORY_NAME { get; set; }
        
    /// <summary>
    ///状态更改次数
    /// </summary>
    public decimal STATUS_UPD_QTY { get; set; }
        
    /// <summary>
    ///状态逻辑值
    /// </summary>
    public short LOGIC_STATUS { get; set; }
        
    /// <summary>
    ///不良品数据ID
    /// </summary>
    public string BAD_DATA_ID { get; set; }
        
    /// <summary>
    ///生产线编码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
        
    /// <summary>
    ///状态更改人
    /// </summary>
    public string STATUS_CHANGE_MAN { get; set; }
        
    /// <summary>
    ///状态更改时间
    /// </summary>
    public System.DateTime CHANGE_TIME { get; set; }
        
    /// <summary>
    ///责任部门
    /// </summary>
    public string DUTY_DEPT { get; set; }
        
    /// <summary>
    ///变更批次
    /// </summary>
    public string CHANGE_BAT { get; set; }
        
    /// <summary>
    ///变更备注
    /// </summary>
    public string CHANGE_REMARK { get; set; }
        
    /// <summary>
    ///状态更改来源
    /// </summary>
    public string STATUS_CHANGE_SOURCE { get; set; }
        
    /// <summary>
    ///RECEIVE_TIME
    /// </summary>
    public System.DateTime RECEIVE_TIME { get; set; }
        
    /// <summary>
    ///MAC_TYPE
    /// </summary>
    public string MAC_TYPE { get; set; }
        
    /// <summary>
    ///CYLINDER_HOLE_GRADE
    /// </summary>
    public string CYLINDER_HOLE_GRADE { get; set; }
        
    /// <summary>
    ///CYLINDER_JR_GRADE
    /// </summary>
    public string CYLINDER_JR_GRADE { get; set; }
        
    /// <summary>
    ///CRANKSHAFT_JRPIN_GRADE
    /// </summary>
    public string CRANKSHAFT_JRPIN_GRADE { get; set; }
        
    /// <summary>
    ///部品名称
    /// </summary>
    public string PART_NAME { get; set; }
    
        public virtual T_PQ_BU_E_ASSE_BAT T_PQ_BU_E_ASSE_BAT { get; set; }
    }
}
