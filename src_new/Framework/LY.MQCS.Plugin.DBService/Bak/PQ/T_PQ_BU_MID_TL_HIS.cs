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
    ///中间试漏历史表
    /// </summary>
    public partial class T_PQ_BU_MID_TL_HIS
    {
        
    /// <summary>
    ///中间试漏历史表ID
    /// </summary>
    public string MID_TL_HIS_ID { get; set; }
        
    /// <summary>
    ///部品类型
    /// </summary>
    public string PART_TYPE { get; set; }
        
    /// <summary>
    ///部品二维码
    /// </summary>
    public string PART_2D_CODE { get; set; }
        
    /// <summary>
    ///中间试漏时间
    /// </summary>
    public System.DateTime MID_TL_TIME { get; set; }
        
    /// <summary>
    ///水道泄漏量
    /// </summary>
    public decimal CHAN_LEAKAGE { get; set; }
        
    /// <summary>
    ///油道泄漏量
    /// </summary>
    public decimal OIL_LEAKAGE { get; set; }
        
    /// <summary>
    ///曲轴箱泄漏量
    /// </summary>
    public decimal CRANKCASE_LEAKAGE { get; set; }
        
    /// <summary>
    ///火花塞孔泄漏量
    /// </summary>
    public decimal SPARK_PLUG_LEAKAGE { get; set; }
        
    /// <summary>
    ///摇臂室泄漏量
    /// </summary>
    public decimal ROCKER_CHAMBER_LEAKAGE { get; set; }
        
    /// <summary>
    ///最终试漏泄漏量(堵盖)
    /// </summary>
    public decimal FINAL_TL_COVER { get; set; }
        
    /// <summary>
    ///最终试漏泄漏量(水道)
    /// </summary>
    public decimal FINAL_TL_CHAN { get; set; }
        
    /// <summary>
    ///最终试漏泄漏量(螺堵)
    /// </summary>
    public decimal FINAL_TL_SCREW { get; set; }
        
    /// <summary>
    ///最终试漏泄漏量
    /// </summary>
    public decimal FINAL_TL_AMT { get; set; }
        
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
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///中间试漏次数
    /// </summary>
    public int MID_TL_NUM { get; set; }
        
    /// <summary>
    ///试漏结果
    /// </summary>
    public string TL_RST { get; set; }
        
    /// <summary>
    ///生产线编码
    /// </summary>
    public string PRO_LINE_CODE { get; set; }
    }
}