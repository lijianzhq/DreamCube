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
    ///检出率数据表
    /// </summary>
    public partial class T_PQ_BU_CHECK_RATE_DATA
    {
        
    /// <summary>
    ///检出率数据表ID
    /// </summary>
    public string CHECK_RATE_DATA_ID { get; set; }
        
    /// <summary>
    ///工厂编码（SAP_FACTORY_CODE）
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///班组编码（lookup_value_code）
    /// </summary>
    public string TEAM_CODE { get; set; }
        
    /// <summary>
    ///工位（lookup_value_code）
    /// </summary>
    public string WORK_STATION_CODE { get; set; }
        
    /// <summary>
    ///人员（user_name）
    /// </summary>
    public string STAFF_CODE { get; set; }
        
    /// <summary>
    ///实绩
    /// </summary>
    public decimal ACT_PERF { get; set; }
        
    /// <summary>
    ///年份
    /// </summary>
    public short YEAR_VAL { get; set; }
        
    /// <summary>
    ///月份
    /// </summary>
    public byte MONTH_VAL { get; set; }
    }
}