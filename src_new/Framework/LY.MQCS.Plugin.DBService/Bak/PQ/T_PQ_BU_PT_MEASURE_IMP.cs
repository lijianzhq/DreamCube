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
    ///涂装测量数据导入表
    /// </summary>
    public partial class T_PQ_BU_PT_MEASURE_IMP
    {
        
    /// <summary>
    ///涂装测量数据明细表ID
    /// </summary>
    public string PT_MEASURE_IMP_ID { get; set; }
        
    /// <summary>
    ///涂装测量类型 1：NID、2：膜厚；3：电泳漆；4：单品；5：联合
    /// </summary>
    public string PAINT_MEAS_TYPE { get; set; }
        
    /// <summary>
    ///环境 涂装测量类型为5【 联合】时，0：室内、1：室外，其它为空
    /// </summary>
    public string ENTIRONMENT { get; set; }
        
    /// <summary>
    ///测量项目大类 0：仪器、1：目视
    /// </summary>
    public string ITEM_BIG_TYPE { get; set; }
        
    /// <summary>
    ///测量项目小类 0：正面、1：反面
    /// </summary>
    public string ITEM_SMALL_TYPE { get; set; }
        
    /// <summary>
    ///测量项目
    /// </summary>
    public string MEASURE_ITEM { get; set; }
        
    /// <summary>
    ///部位
    /// </summary>
    public string PART_POS { get; set; }
        
    /// <summary>
    ///测量值
    /// </summary>
    public string MEASURE_VAL { get; set; }
        
    /// <summary>
    ///检规上限
    /// </summary>
    public string CHECK_M_VALUE { get; set; }
        
    /// <summary>
    ///检规下限
    /// </summary>
    public string CHECK_L_VALUE { get; set; }
        
    /// <summary>
    ///内规上限
    /// </summary>
    public string IN_MAX_VALUE { get; set; }
        
    /// <summary>
    ///内规下限
    /// </summary>
    public string IN_L_BOUND_VAL { get; set; }
        
    /// <summary>
    ///部位大类 1：车身、2：保杠
    /// </summary>
    public string PART_POS_BIG_TYPE { get; set; }
        
    /// <summary>
    ///部位中类 H：水平、V：垂直
    /// </summary>
    public string PART_POS_MID_TYPE { get; set; }
        
    /// <summary>
    ///部位小类 1：LF,2：LR,3：RL,4：RR
    /// </summary>
    public string PART_POS_SMALL_TYPE { get; set; }
        
    /// <summary>
    ///创建人
    /// </summary>
    public string CREATOR { get; set; }
        
    /// <summary>
    ///创建日期
    /// </summary>
    public System.DateTime CREATED_DATE { get; set; }
        
    /// <summary>
    ///行号
    /// </summary>
    public int ROW_NUMBER { get; set; }
        
    /// <summary>
    ///涂装测量数据导入错误类型 1：中断；2：提示；默认：1
    /// </summary>
    public string PT_ERR_FLAG { get; set; }
        
    /// <summary>
    ///涂装测量数据导入错误信息
    /// </summary>
    public string PT_ERR_MSG { get; set; }
        
    /// <summary>
    ///批次号
    /// </summary>
    public string BATCHNO { get; set; }
    }
}
