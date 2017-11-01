using System;

namespace DreamCube.Foundation.Basic.Objects
{
    /// <summary>
    /// 弱引用对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct WeakReference<T> : IDisposable where T : class
    {
        #region "字段"

        private WeakReference innerObj;

        private T target;

        /// <summary>
        /// 标识当前是否是强引用
        /// </summary>
        private Boolean isStrongReference;

        #endregion

        public WeakReference(T target)
            : this(target, false)
        { }

        /// <summary>
        /// 升级为强引用
        /// </summary>
        /// <param name="target"></param>
        /// <param name="upToStrongReference"></param>
        public WeakReference(T target, Boolean upToStrongReference)
        {
            if (upToStrongReference)
            {
                this.isStrongReference = true;
                this.target = target;
                this.innerObj = null;
            }
            else
            {
                this.innerObj = new WeakReference(target);
                this.isStrongReference = false;
                this.target = null;
            }
        }

        /// <summary>
        /// 当对象还存在的时候，调用此方法才有效果的，否则没效果的
        /// </summary>
        public void UpToStrongReference()
        {
            if (!this.isStrongReference)
            {
                if (this.innerObj.IsAlive)
                {
                    this.target = this.innerObj as T;
                    this.innerObj = null;
                    this.isStrongReference = true;
                }
            }
        }

        /// <summary>
        /// 从强引用降级为弱引用
        /// </summary>
        public void DownToWeakReference()
        {
            if (this.isStrongReference)
            {
                this.innerObj = new WeakReference(this.target);
                this.isStrongReference = false;
                this.target = null;
            }
        }

        /// <summary>
        /// 弱引用指向的真实对象
        /// </summary>
        public T Target
        {
            get
            {
                if (isStrongReference) return target;

                if (innerObj.IsAlive)
                    return innerObj.Target as T;
                return null;
            }
        }

        /// <summary>
        /// 实现接口方法，调用释放引用
        /// </summary>
        public void Dispose()
        {
            this.innerObj = null;
        }
    }
}
