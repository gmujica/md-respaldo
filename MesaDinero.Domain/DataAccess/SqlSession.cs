using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace MesaDinero.Domain.DataAccess
{
    public sealed class SqlSession : IDisposable
    {
        private SqlConnection _sqlConnection;
        private bool disposed;
        [ThreadStatic]
        private static bool? shouldImpersonate = null;

        internal SqlSession()
            : this(ConnectionInfo.MesaDineroDB)
        {
        }

        internal SqlSession(string connectString)
        {
            this.ConnectionString = connectString;
            this.DoImpersonate();
        }

        public void Close()
        {
            if (this._sqlConnection != null)
            {
                this._sqlConnection.Close();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this._sqlConnection != null)
                    {
                        if (this._sqlConnection.State != ConnectionState.Closed)
                        {
                            this._sqlConnection.Close();
                        }
                        this._sqlConnection.Dispose();
                    }
                    if (this.ImpersonationContext != null)
                    {
                        this.ImpersonationContext.Undo();
                        this.ImpersonationContext.Dispose();
                    }
                }
                this.disposed = true;
            }
        }

        private void DoImpersonate()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent(true);
            if ((!string.IsNullOrEmpty(this.ConnectionString) && Impersonate) && (current != null))
            {
                this.ImpersonationContext = WindowsIdentity.Impersonate(IntPtr.Zero);
            }
        }

        public void Open()
        {
            if (this._sqlConnection != null)
            {
                try
                {
                    this._sqlConnection.Open();
                }
                catch (SqlException exception)
                {
                    throw exception;
                }
            }
        }

        public SqlConnection Connection
        {
            get
            {
                if ((this._sqlConnection == null) && !string.IsNullOrEmpty(this.ConnectionString))
                {
                    this._sqlConnection = new SqlConnection(this.ConnectionString);
                }
                return this._sqlConnection;
            }
        }

        public string ConnectionString { get; private set; }

        public static bool Impersonate
        {
            get
            {
                bool? shouldImpersonate = SqlSession.shouldImpersonate;
                return (!shouldImpersonate.HasValue || shouldImpersonate.GetValueOrDefault());
            }
            set
            {
                shouldImpersonate = new bool?(value);
            }
        }

        public WindowsImpersonationContext ImpersonationContext { get; set; }
    }
}
