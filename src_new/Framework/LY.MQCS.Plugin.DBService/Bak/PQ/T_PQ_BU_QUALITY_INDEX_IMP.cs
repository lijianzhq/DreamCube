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
    ///指标实绩值导入表
    /// </summary>
    public partial class T_PQ_BU_QUALITY_INDEX_IMP
    {
        
    /// <summary>
    ///指标实绩值导入表ID
    /// </summary>
    public string QUALITY_INDEX_IMP_ID { get; set; }
        
    /// <summary>
    ///工厂编码
    /// </summary>
    public string FACTORY_CODE { get; set; }
        
    /// <summary>
    ///车型编码
    /// </summary>
    public string CAR_TYPE_CODE { get; set; }
        
    /// <summary>
    ///日期
    /// </summary>
    public string DATE_VAL { get; set; }
        
    /// <summary>
    ///指标类型
    /// </summary>
    public string INDEX_TYPE { get; set; }
        
    /// <summary>
    ///工程别
    /// </summary>
    public string PROJ_TYPE { get; set; }
        
    /// <summary>
    ///管理项目
    /// </summary>
    public string MANAGE_PROJ { get; set; }
        
    /// <summary>
    ///分子值
    /// </summary>
    public decimal MOLECULAR_VAL { get; set; }
        
    /// <summary>
    ///分母值
    /// </summary>
    public decimal DENOMINATOR_VAL { get; set; }
        
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
    public string ERROR_MESSAGE { get; set; }
    }
}
