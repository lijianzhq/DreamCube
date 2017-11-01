using System;
using System.Diagnostics;

namespace DreamCube.Foundation.LogService
{
    public struct AutoLoggerGuard : IDisposable
    {
        #region constructor

        public AutoLoggerGuard( LoggerWrapper argLogger,
                                string argScope)
        {
            Debug.Assert( null != argLogger );
 
            m_logger = argLogger;
            m_scope = argScope;
            m_logger.LogDebugFormat("Enter {0}", m_scope);
        }

        #endregion

        #region method

        void IDisposable.Dispose()
        {
            Debug.Assert(null != m_logger);

            m_logger.LogDebugFormat("Leave {0}", m_scope);
        }

        #endregion

        #region field

        private string m_scope;

        private LoggerWrapper m_logger; 

        #endregion
    }
}
