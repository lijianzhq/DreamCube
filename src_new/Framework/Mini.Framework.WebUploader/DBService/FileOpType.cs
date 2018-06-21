using System;

namespace Mini.Framework.WebUploader.DBService
{
    public enum FileOpType
    {
        Create = 0, //创建
        Delete = 99, //删除
        View = 1, //查看
        Download = 2 //下载
    }
}
