using System;

namespace Mini.Framework.WebUploader.DBService
{
    public enum FileStatus
    {
        Normal = 0, //正常状态
        Delete = 99, //删除状态 
        Disabled = 1, //禁用状态
    }
}
