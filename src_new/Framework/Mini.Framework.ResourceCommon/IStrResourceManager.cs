using System;

namespace Mini.Framework.ResourceCommon
{
    public interface IStrResourceManager
    {
        String GetString(String key, String ns = "", String language = "");
    }
}
