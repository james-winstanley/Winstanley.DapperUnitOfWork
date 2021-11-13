using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Dapper.FastCrud;
using Dapper.FastCrud.Configuration;
using Dapper.FastCrud.Configuration.StatementOptions.Builders;
using Winstanley.UnitOfWork.Interfaces;

namespace Winstanley.UnitOfWork.Repositories
{
    public class Repository<T> : OrmConventions, IRepository<T>
        where T : class
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="transaction">The database connection transaction.</param>
        /// <exception cref="System.ArgumentNullException">transaction</exception>
        public Repository(IDbTransaction transaction)
        {
            Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            OrmConfiguration.DefaultDialect = SqlDialect.MsSql;
        }


        /// <summary>
        /// Initialises a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="transaction">The database connection transaction.</param>
        /// <param name="dialect">The SQL language dialect.</param>
        /// <exception cref="System.ArgumentNullException">transaction</exception>
        public Repository(IDbTransaction transaction, SqlDialect dialect)
        {
            Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            OrmConfiguration.DefaultDialect = dialect;
        }


        protected IDbTransaction Transaction { get; }
        protected IDbConnection Connection => Transaction.Connection;


        #region Dapper

        /// <summary>
        /// Executes the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of rows affected.</returns>
        public virtual int Execute(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return Connection.Execute(new CommandDefinition(sql, parameters, Transaction, null, commandType, cancellationToken: cancellationToken));
        }


        /// <summary>
        /// Executes the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of rows affected.</returns>
        public virtual async Task<int> ExecuteAsync(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return await Connection.ExecuteAsync(new CommandDefinition(sql, parameters, Transaction, commandType: commandType, cancellationToken: cancellationToken));
        }


        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="parameters">The parameters to pass, if any.</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A sequence of data of the supplied type; if a basic type (int, string, etc) is
        /// queried then the data from the first column in assumed, otherwise an instance
        /// is created per row, and a direct column-name===member-name mapping is assumed
        /// (case insensitive).
        /// </returns>
        public virtual IEnumerable<T> Query(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return Connection.Query<T>(new CommandDefinition(sql, parameters, Transaction, commandType: commandType, cancellationToken: cancellationToken));
        }


        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="parameters">The parameters to pass, if any.</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A sequence of data of the supplied type; if a basic type (int, string, etc) is
        /// queried then the data from the first column in assumed, otherwise an instance
        /// is created per row, and a direct column-name===member-name mapping is assumed
        /// (case insensitive).
        /// </returns>
        public virtual async Task<IEnumerable<T>> QueryAsync(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return await Connection.QueryAsync<T>(new CommandDefinition(sql, parameters, Transaction, commandType: commandType, cancellationToken: cancellationToken));
        }


        /// <summary>
        /// Queries the int.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="parameters">The parameters to pass, if any.</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<int>> QueryInt(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return await Connection.QueryAsync<int>(new CommandDefinition(sql, parameters, Transaction, commandType: commandType, cancellationToken: cancellationToken));
        }


        /// <summary>
        /// Queries the int asynchronous.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="parameters">The parameters to pass, if any.</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<int>> QueryIntAsync(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            return await Connection.QueryAsync<int>(new CommandDefinition(sql, parameters, Transaction, commandType: commandType, cancellationToken: cancellationToken));
        }

        #endregion Dapper


        #region Dapper.FastCrud

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Identity of the inserted entity.</returns>
        public virtual int Insert(T entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
                ((IAuditEntity)entity).CreatedAt = DateTime.UtcNow;

            Connection.Insert(entity, statementOptions => statementOptions.AttachToTransaction(Transaction));
            return Connection.Query<int>("IF @@IDENTITY IS NULL SELECT 0 ELSE SELECT @@IDENTITY", transaction: Transaction).First();
        }


        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Identity of the inserted entity.</returns>
        public virtual async Task<int> InsertAsync(T entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
                ((IAuditEntity)entity).CreatedAt = DateTime.UtcNow;

            await Connection.InsertAsync(entity, statementOptions => statementOptions.AttachToTransaction(Transaction));
            return (await Connection.QueryAsync<int>("IF @@IDENTITY IS NULL SELECT 0 ELSE SELECT @@IDENTITY", transaction: Transaction)).First();
        }


        /// <summary>
        /// Gets the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity.</returns>
        public virtual T Get(T entity)
        {
            return Connection.Get(entity, statementOptions => statementOptions.AttachToTransaction(Transaction));
        }


        /// <summary>
        /// Gets the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity.</returns>
        public virtual async Task<T> GetAsync(T entity)
        {
            return await Connection.GetAsync(entity, statementOptions => statementOptions.AttachToTransaction(Transaction));
        }


        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IEnumerable entities.</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return Connection.Find<T>(statementOptions => statementOptions.AttachToTransaction(Transaction));
        }


        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IEnumerable entities.</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Connection.FindAsync<T>(statementOptions => statementOptions.AttachToTransaction(Transaction));
        }


        /// <summary>
        /// Performs a find query with the specified statement options.
        /// </summary>
        /// <param name="statementOptions">The statement options.</param>
        /// <returns>IEnumerable entities.</returns>
        public virtual IEnumerable<T> Find(Action<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<T>> statementOptions = null)
        {
            if (statementOptions != null)
                statementOptions += x => x.AttachToTransaction(Transaction);

            return Connection.Find(statementOptions);
        }


        /// <summary>
        /// Performs a find query with the specified statement options.
        /// </summary>
        /// <param name="statementOptions">The statement options.</param>
        /// <returns>IEnumerable entities.</returns>
        public virtual async Task<IEnumerable<T>> FindAsync(Action<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<T>> statementOptions = null)
        {
            if (statementOptions != null)
                statementOptions += x => x.AttachToTransaction(Transaction);

            return await Connection.FindAsync(statementOptions);
        }


        /// <summary>
        /// Counts all the records in a table or a range of records if a conditional clause was set up in the statementOptions options.
        /// </summary>
        /// <param name="statementOptions">Optional statementOptions options (usage: statementOptions =&gt; statementOptions.SetTimeout().AttachToTransaction()...)</param>
        /// <returns>The record count.</returns>
        public virtual int Count(Action<IConditionalSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (statementOptions != null)
                statementOptions += x => x.AttachToTransaction(Transaction);

            return Connection.Count(statementOptions);
        }


        /// <summary>
        /// Counts all the records in a table or a range of records if a conditional clause was set up in the statementOptions options.
        /// </summary>
        /// <param name="statementOptions">Optional statementOptions options (usage: statementOptions =&gt; statementOptions.SetTimeout().AttachToTransaction()...)</param>
        /// <returns>The record count.</returns>
        public virtual async Task<int> CountAsync(Action<IConditionalSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (statementOptions != null)
                statementOptions += x => x.AttachToTransaction(Transaction);

            return await Connection.CountAsync(statementOptions);
        }


        /// <summary>
        /// CAUTION: Using this method will cause data loss if object models are not kept up to date!
        /// Updates a record in the database.
        /// </summary>
        /// <param name="entity">The entity you wish to update.</param>
        /// <returns>True if the item was updated.</returns>
        public virtual bool Update(T entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
                ((IAuditEntity)entity).UpdatedAt = DateTime.UtcNow;

            return Connection.Update(entity, statementOptions => statementOptions.AttachToTransaction(Transaction));
        }


        /// <summary>
        /// CAUTION: Using this method will cause data loss if object models are not kept up to date!
        /// Updates a record in the database.
        /// </summary>
        /// <param name="entity">The entity you wish to update.</param>
        /// <returns>True if the item was updated.</returns>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
                ((IAuditEntity)entity).UpdatedAt = DateTime.UtcNow;

            return await Connection.UpdateAsync(entity, statementOptions => statementOptions.AttachToTransaction(Transaction));
        }


        /// <summary>
        /// Bulks the update.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="statementOptions">The statement options.</param>
        /// <returns></returns>
        public virtual int BulkUpdate(T obj, Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions)
        {
            return Connection.BulkUpdate(obj, statementOptions);
        }


        /// <summary>
        /// Bulks the update asynchronous.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="statementOptions">The statement options.</param>
        /// <returns></returns>
        public virtual async Task<int> BulkUpdateAsync(T obj, Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions)
        {
            return await Connection.BulkUpdateAsync(obj, statementOptions);
        }


        /// <summary>
        /// Deletes the specified object.
        /// WARNING: Data will be lost and will not be recoverable!
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if object is deleted, else <c>false</c>.
        /// </returns>
        public virtual bool Delete(T obj)
        {
            if (!typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException($"{typeof(T).Name} is not of type {nameof(IDeleteEntity)} and cannot be deleted.");

            return Connection.Delete(obj, statementOptions => statementOptions.AttachToTransaction(Transaction));
        }


        /// <summary>
        /// Deletes the specified object.
        /// WARNING: Data will be lost and will not be recoverable!
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if object is deleted, else <c>false</c>.
        /// </returns>
        public virtual async Task<bool> DeleteAsync(T obj)
        {
            if (!typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException($"{typeof(T).Name} is not of type {nameof(IDeleteEntity)} and cannot be deleted.");

            return await Connection.DeleteAsync(obj, statementOptions => statementOptions.AttachToTransaction(Transaction));
        }

        #endregion Dapper.FastCrud
    }
}