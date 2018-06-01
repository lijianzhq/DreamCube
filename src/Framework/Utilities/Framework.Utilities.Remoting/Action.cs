using System;
using System.Collections.Generic;
using System.Text;

namespace DreamCube.Framework.Utilities.Remoting
{

    //在.NET2.0中需要自定义这个委托
#if NET20

    public delegate void Action<T1, T2>(T1 t1, T2 t2);
    public delegate void Action();

#endif

}
