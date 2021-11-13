using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Winstanley.UnitOfWork.Interfaces;

namespace Winstanley.UnitOfWork
{
    public abstract class UnitOfWorkBase : IUnitOfWorkBase
    {
        protected UnitOfWorkBase(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("A database connection string has not been provided", nameof(connectionString));

            _connection = new SqlConnection(connectionString);
            _connection.Open();
            Transaction = _connection.BeginTransaction();
        }


        private IDbConnection _connection;
        private bool _disposed;


        protected IDbTransaction Transaction { get; private set; }


        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void Commit()
        {
            try
            {
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
            finally
            {
                Transaction.Dispose();
                Transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }


        /// <summary>
        /// Resets the repositories.
        /// </summary>
        protected abstract void ResetRepositories();


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Transaction != null)
                    {
                        Transaction.Rollback(); // Added to ensure uncommitted transactions are rolled back
                        Transaction.Dispose();
                        Transaction = null;
                    }

                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }

                _disposed = true;
            }
        }


        ~UnitOfWorkBase()
        {
            Dispose(false);
        }
    }
}