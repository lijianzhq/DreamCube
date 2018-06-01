using System;
using System.Globalization;
using System.Text;

namespace DreamCube.Framework.DataAccess.Basic
{
	/// <summary>
	/// 连接字符串对象
	/// </summary>
	public class ConnectionString
    {
        #region "字段"

        private const Char CONNSTRING_DELIM = ';';
		private String connectionString;
		private String connectionStringWithoutCredentials;
		private String userIdTokens;
		private String passwordTokens;
        private String datasourceTokens;
        private DBProviderType dbProviderType = DBProviderType.SqlClient;

        #endregion

        #region "属性"

        /// <summary>
        /// 数据库提供程序
        /// </summary>
        public DBProviderType DBProviderType
        {
            get { return dbProviderType; }
        }

        /// <summary>
        /// 数据库提供程序（字符串形式）
        /// </summary>
        public String DBProviderTypeEx
        {
            get { return dbProviderType.ToString(); }
        }

        /// <summary>
        /// 获取DataSource 
        /// </summary>
        public String DataSource
        {
            get
            {
                String lowConnString = connectionString.ToLowerInvariant();
                Int32 uidPos;
                Int32 uidMPos;
                GetTokenPositions(datasourceTokens, out uidPos, out uidMPos);
                if (0 <= uidPos)
                {
                    Int32 uidEPos = lowConnString.IndexOf(CONNSTRING_DELIM, uidMPos);
                    return connectionString.Substring(uidMPos, uidEPos - uidMPos);
                }
                else return String.Empty;
            }
        }

        /// <summary>
        /// 抽取连接字符串中的用户名
        /// </summary>
        public String UserName
        {
            get
            {
                String lowConnString = connectionString.ToLowerInvariant();
                Int32 uidPos;
                Int32 uidMPos;
                GetTokenPositions(userIdTokens, out uidPos, out uidMPos);
                if (0 <= uidPos)
                {
                    Int32 uidEPos = lowConnString.IndexOf(CONNSTRING_DELIM, uidMPos);
                    return connectionString.Substring(uidMPos, uidEPos - uidMPos);
                }
                else  return String.Empty;
            }
            set
            {
                String lowConnString = connectionString.ToLowerInvariant();
                Int32 uidPos;
                Int32 uidMPos;
                GetTokenPositions(userIdTokens, out uidPos, out uidMPos);
                if (0 <= uidPos)
                {
                    // found a user id, so replace the value
                    Int32 uidEPos = lowConnString.IndexOf(CONNSTRING_DELIM, uidMPos);
                    connectionString = connectionString.Substring(0, uidMPos) +
                        value + connectionString.Substring(uidEPos);
                    //_connectionStringNoCredentials = RemoveCredentials(_connectionString);
                }
                else
                {
                    //no user id in the connection string so just append to the connection string
                    String[] tokens = userIdTokens.Split(',');
                    connectionString += tokens[0] + value + CONNSTRING_DELIM;
                }
            }
        }

        /// <summary>
        /// 抽取连接字符串中的密码
        /// </summary>
        protected String Password
        {
            get
            {
                String lowConnString = connectionString.ToLowerInvariant();
                Int32 pwdPos;
                Int32 pwdMPos;
                GetTokenPositions(passwordTokens, out pwdPos, out pwdMPos);

                if (0 <= pwdPos)
                {
                    Int32 pwdEPos = lowConnString.IndexOf(CONNSTRING_DELIM, pwdMPos);
                    return connectionString.Substring(pwdMPos, pwdEPos - pwdMPos);
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                String lowConnString = connectionString.ToLowerInvariant();
                Int32 pwdPos;
                Int32 pwdMPos;
                GetTokenPositions(passwordTokens, out pwdPos, out pwdMPos);

                if (0 <= pwdPos)
                {
                    // found a password, so replace the value
                    Int32 pwdEPos = lowConnString.IndexOf(CONNSTRING_DELIM, pwdMPos);
                    connectionString = connectionString.Substring(0, pwdMPos) + value + connectionString.Substring(pwdEPos);
                    //_connectionStringNoCredentials = RemoveCredentials(_connectionString);
                }
                else
                {
                    //no password in the connection string so just append to the connection string
                    String[] tokens = passwordTokens.Split(',');
                    connectionString += tokens[0] + value + CONNSTRING_DELIM;
                }
            }
        }

        #endregion 

        #region "公共方法"

        /// <summary>
        /// 连接字符串对象构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="userIdTokens"></param>
        /// <param name="passwordTokens"></param>
        /// <param name="datasourceTokens"></param>
        internal ConnectionString(String connectionString, String userIdTokens, String passwordTokens, String datasourceTokens)
            : this(connectionString, userIdTokens, passwordTokens, datasourceTokens, DBProviderType.SqlClient)
        { }

        /// <summary>
        /// 连接字符串对象构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        internal ConnectionString(String connectionString)
            : this(connectionString, Properties.Resources.UserNameToken,
                  Properties.Resources.PasswordToken, Properties.Resources.DataSourceToken)
        { }

        /// <summary>
        /// 连接字符串对象构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="providerType">数据库提供程序</param>
        internal ConnectionString(String connectionString, DBProviderType providerType)
            : this(connectionString, Properties.Resources.UserNameToken,
                  Properties.Resources.PasswordToken, Properties.Resources.DataSourceToken, providerType)
        { }

        internal ConnectionString(String connectionString, String userIdTokens, String passwordTokens, String datasourceTokens, DBProviderType dbProviderType)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentException(Properties.Resources.ExceptionNullOrEmptyString, "connectionString");
            if (String.IsNullOrEmpty(userIdTokens))
                throw new ArgumentException(Properties.Resources.ExceptionNullOrEmptyString, "userIdTokens");
            if (String.IsNullOrEmpty(passwordTokens))
                throw new ArgumentException(Properties.Resources.ExceptionNullOrEmptyString, "passwordTokens");
            if (String.IsNullOrEmpty(datasourceTokens))
                throw new ArgumentException(Properties.Resources.ExceptionNullOrEmptyString, "datasourceTokens");

            this.dbProviderType = dbProviderType;
            this.connectionString = connectionString;
            this.userIdTokens = userIdTokens;
            this.passwordTokens = passwordTokens;
            this.datasourceTokens = datasourceTokens;
            this.connectionStringWithoutCredentials = null;
        }

        /// <summary>
        /// 返回连接字符串
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return connectionString;
        }

        /// <summary>
        /// 返回连接字符串（去掉用户名和密码）
        /// </summary>
        /// <returns></returns>
        public String ToStringNoCredentials()
        {
            if (connectionStringWithoutCredentials == null)
                connectionStringWithoutCredentials = RemoveCredentials(connectionString);
            return connectionStringWithoutCredentials;
        }

        #endregion

        #region "私有方法"

        /// <summary>
        /// 获取标识符的位置（连接字符串中，属性值对的属性名的位置）
        /// 如果一个连接字符串存在多个相同的属性值对，则程序只取第一个；
        /// 例如：连接字符串同时存在user id= 和 uid= 两个属性对时，则取前面的属性对的属性值
        /// </summary>
        /// <param name="tokenString"></param>
        /// <param name="tokenPos">属性值名称、标识符起始位置</param>
        /// <param name="tokenMPos">属性值名称、标识符结束位置</param>
        private void GetTokenPositions(String tokenString, out Int32 tokenPos, out Int32 tokenMPos)
		{
            String[] tokens = tokenString.Split(',');
            Int32 currentPos = -1;
            String lowConnString = connectionString.ToLowerInvariant();

			//初始化输出参数
			tokenPos = -1;
			tokenMPos = -1;
			foreach (String token in tokens)
			{
				currentPos = lowConnString.IndexOf(token);
                if (currentPos >= 0)
                {
                    tokenPos = currentPos;
                    tokenMPos = currentPos + token.Length;
                    break;
                }
			}
		}

        /// <summary>
        /// 移除连接字符串中的认证信息
        /// </summary>
        /// <param name="connectionStringToModify"></param>
        /// <returns></returns>
        private String RemoveCredentials(String connectionStringToModify)
		{
			StringBuilder connStringNoCredentials = new StringBuilder();
            String[] tokens = connectionStringToModify.ToLowerInvariant().Split(CONNSTRING_DELIM);
            String thingsToRemove = userIdTokens + "," + passwordTokens;
            String[] avoidTokens = thingsToRemove.ToLowerInvariant().Split(',');

			foreach (String section in tokens)
			{
				Boolean found = false;
				String token = section.Trim();
				if (token.Length != 0)
				{
					foreach (String avoidToken in avoidTokens)
					{
						if (token.StartsWith(avoidToken))
						{
							found = true;
							break;
						}
					}
					if (!found)
					{
						connStringNoCredentials.Append(token + CONNSTRING_DELIM);
					}
				}
			}
			return connStringNoCredentials.ToString();
        }

        #endregion
    }
}
