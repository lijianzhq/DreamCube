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
    ///空白条码匹配对象表
    /// </summary>
    public partial class T_PQ_BU_BLANK_BAR_TMP
    {
        
    /// <summary>
    ///空白条码匹配对象ID
    /// </summary>
    public string BLANK_BAR_TMP_ID { get; set; }
        
    /// <summary>
    ///部件临时编码 临时二维码
    /// </summary>
    public string PART_TMP_CODE { get; set; }
        
    /// <summary>
    ///匹配部品二维码
    /// </summary>
    public string MATCH_PART_2D_CODE { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///生产线编码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
        
    /// <summary>
    ///部品类型
    /// </summary>
    public string PART_TYPE { get; set; }
        
    /// <summary>
    ///部品状态
    /// </summary>
    public string PART_STATUS { get; set; }
        
    /// <summary>
    ///当前工序编码 当前工序（临时二维码匹配工序）
    /// </summary>
    public string CURR_PROCESS_CODE { get; set; }
        
    /// <summary>
    ///当前工序设备号
    /// </summary>
    public string CURR_EQUIP_CODE { get; set; }
        
    /// <summary>
    ///当前工序过机时间
    /// </summary>
    public System.DateTime CURR_TIME { get; set; }
        
    /// <summary>
    ///当前工序状态
    /// </summary>
    public string CURR_STATUS { get; set; }
        
    /// <summary>
    ///上工序编码
    /// </summary>
    public string PREV_PROCESS_CODE { get; set; }
        
    /// <summary>
    ///上工序设备号
    /// </summary>
    public string PREV_EQUIP_CODE { get; set; }
        
    /// <summary>
    ///上工序过机时间
    /// </summary>
    public System.DateTime PREV_TIME { get; set; }
        
    /// <summary>
    ///上工序状态
    /// </summary>
    public string PREV_STATUS { get; set; }
        
    /// <summary>
    ///下工序编码
    /// </summary>
    public string NEXT_PROCESS_CODE { get; set; }
        
    /// <summary>
    ///下工序设备号
    /// </summary>
    public string NEXT_EQUIP_CODE { get; set; }
        
    /// <summary>
    ///下工序过机时间
    /// </summary>
    public System.DateTime NEXT_TIME { get; set; }
        
    /// <summary>
    ///下工序状态
    /// </summary>
    public string NEXT_STATUS { get; set; }
        
    /// <summary>
    ///上线工序编码
    /// </summary>
    public string ONLINE_PROCESS_CODE { get; set; }
        
    /// <summary>
    ///上线工序设备号
    /// </summary>
    public string ONLINE_P_EQUIP_CODE { get; set; }
        
    /// <summary>
    ///上线工序过机时间
    /// </summary>
    public System.DateTime ONLINE_P_TIME { get; set; }
        
    /// <summary>
    ///上线工序状态
    /// </summary>
    public string ONLINE_P_STATUS { get; set; }
        
    /// <summary>
    ///下线工序编码
    /// </summary>
    public string OFFLINE_PROCESS_CODE { get; set; }
        
    /// <summary>
    ///下线工序设备号
    /// </summary>
    public string OFFLINE_P_EQUIP_CODE { get; set; }
        
    /// <summary>
    ///下线工序过机时间
    /// </summary>
    public System.DateTime OFFLINE_P_TIME { get; set; }
        
    /// <summary>
    ///下线工序状态
    /// </summary>
    public string OFFLINE_P_STATUS { get; set; }
        
    /// <summary>
    ///匹配人
    /// </summary>
    public string MATCH_MAN { get; set; }
        
    /// <summary>
    ///匹配时间
    /// </summary>
    public System.DateTime MATCH_TIME { get; set; }
        
    /// <summary>
    ///匹配批次号
    /// </summary>
    public string MATCH_BATCH_NO { get; set; }
        
    /// <summary>
    ///最新匹配状态 1为最新匹配；0非最新匹配
    /// </summary>
    public string MATCH_STATUS { get; set; }
        
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
    ///部品工程信息ID
    /// </summary>
    public string PART_PROJ_ID { get; set; }
        
    /// <summary>
    ///临时条码部品工程信息ID
    /// </summary>
    public string TMP_PART_PROJ_ID { get; set; }
    }
}
