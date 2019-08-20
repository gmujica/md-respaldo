using System;

namespace MesaDinero.Domain.DataAccess
{
    public static class ConnectionInfo
    {
        internal static SqlSession GetSocialDB_FlujosConnection()
        {
            return new SqlSession(MesaDineroDB);
        }

        internal static string MesaDineroDB
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["MesaDineroContext"].ConnectionString;
            }
        }
    }
}
