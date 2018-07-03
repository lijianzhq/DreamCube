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
    ///工厂品质目标值维护表
    /// </summary>
    public partial class T_PQ_BU_TATGET_IMP
    {
        
    /// <summary>
    ///表ID
    /// </summary>
    public string TATGET_ACTUAL_ID { get; set; }
        
    /// <summary>
    ///工程车间编码
    /// </summary>
    public string WORKSHOP_CODE { get; set; }
        
    /// <summary>
    ///类别编码
    /// </summary>
    public string PROJECT_TYPE_CODE { get; set; }
        
    /// <summary>
    ///管理项目表ID
    /// </summary>
    public string MANAGE_PROJECT_ID { get; set; }
        
    /// <summary>
    ///车型
    /// </summary>
    public string CAR_TYPE_CODE { get; set; }
        
    /// <summary>
    ///年月
    /// </summary>
    public string YEAR_VAL { get; set; }
        
    /// <summary>
    ///必达
    /// </summary>
    public decimal PILDASH { get; set; }
        
    /// <summary>
    ///挑战1
    /// </summary>
    public decimal CHALLENGE_ONE { get; set; }
        
    /// <summary>
    ///挑战2
    /// </summary>
    public decimal CHALLENGE_TWO { get; set; }
        
    /// <summary>
    ///挑战3(预留字段)
    /// </summary>
    public decimal CHALLENGE_FEB { get; set; }
        
    /// <summary>
    ///备注
    /// </summary>
    public string REMARKS { get; set; }
        
    /// <summary>
    ///创建人
    /// </summary>
    public string CREATOR { get; set; }
        
    /// <summary>
    ///导入批次号
    /// </summary>
    public string BATCHNO { get; set; }
        
    /// <summary>
    ///错误信息
    /// </summary>
    public string COLUMN68 { get; set; }
        
    /// <summary>
    ///序号
    /// </summary>
    public string XH_CODE { get; set; }
        
    /// <summary>
    ///录入方式
    /// </summary>
    public string ENTER_WAY { get; set; }
        
    /// <summary>
    ///单位
    /// </summary>
    public string UNIT { get; set; }
    }
}