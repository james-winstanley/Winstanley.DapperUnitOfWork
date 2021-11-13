using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper.FastCrud.Configuration.StatementOptions.Builders;

namespace Winstanley.UnitOfWork.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        #region Dapper

        /// <summary>
        /// Executes the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of rows affected.</returns>
        int Execute(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);


        /// <summary>
        /// Executes the specified SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of rows affected.</returns>
        Task<int> ExecuteAsync(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);


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
        IEnumerable<T> Query(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);
        

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
        Task<IEnumerable<T>> QueryAsync(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);


        /// <summary>
        /// Queries the int.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="parameters">The parameters to pass, if any.</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<IEnumerable<int>> QueryInt(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);


        /// <summary>
        /// Queries the int asynchronous.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="parameters">The parameters to pass, if any.</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<IEnumerable<int>> QueryIntAsync(string sql, object parameters, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default);

        #endregion Dapper


        #region Dapper.FastCrud

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Identity of the inserted entity.</returns>
        int Insert(T entity);


        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Identity of the inserted entity.</returns>
        Task<int> InsertAsync(T entity);


        /// <summary>
        /// Gets the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity.</returns>
        T Get(T entity);


        /// <summary>
        /// Gets the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity.</returns>
        Task<T> GetAsync(T entity);


        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IEnumerable entities.</returns>
        IEnumerable<T> GetAll();


        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IEnumerable entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();


        /// <summary>
        /// Performs a find query with the specified statement options.
        /// </summary>
        /// <param name="statementOptions">The statement options.</param>
        /// <returns>IEnumerable entities.</returns>
        IEnumerable<T> Find(Action<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<T>> statementOptions = null);


        /// <summary>
        /// Performs a find query with the specified statement options.
        /// </summary>
        /// <param name="statementOptions">The statement options.</param>
        /// <returns>IEnumerable entities.</returns>
        Task<IEnumerable<T>> FindAsync(Action<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<T>> statementOptions = null);


        /// <summary>
        /// Counts all the records in a table or a range of records if a conditional clause
        /// was set up in the statementOptions options.
        /// </summary>
        /// <param name="statementOptions">Optional statementOptions options (usage: statementOptions => statementOptions.SetTimeout().AttachToTransaction()...)</param>
        /// <returns>The record count.</returns>
        int Count(Action<IConditionalSqlStatementOptionsBuilder<T>> statementOptions = null);


        /// <summary>
        /// Counts all the records in a table or a range of records if a conditional clause
        /// was set up in the statementOptions options.
        /// </summary>
        /// <param name="statementOptions">Optional statementOptions options (usage: statementOptions => statementOptions.SetTimeout().AttachToTransaction()...)</param>
        /// <returns>The record count.</returns>
        Task<int> CountAsync(Action<IConditionalSqlStatementOptionsBuilder<T>> statementOptions = null);


        /// <summary>
        /// CAUTION: Using this method will cause data loss if object models are not kept up to date!
        /// Updates a record in the database.
        /// </summary>
        /// <param name="entity">The entity you wish to update.</param>
        /// <returns>True if the item was updated.</returns>
        bool Update(T entity);


        /// <summary>
        /// CAUTION: Using this method will cause data loss if object models are not kept up to date!
        /// Updates a record in the database.
        /// </summary>
        /// <param name="entity">The entity you wish to update.</param>
        /// <returns>True if the item was updated.</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Bulks the update.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="statementOptions">The statement options.</param>
        /// <returns></returns>
        int BulkUpdate(T obj, Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions);


        /// <summary>
        /// Bulks the update asynchronous.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="statementOptions">The statement options.</param>
        /// <returns></returns>
        Task<int> BulkUpdateAsync(T obj, Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions);


        /// <summary>
        /// Deletes the specified object.
        /// WARNING: Data will be lost and will not be recoverable!
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if object is deleted, else <c>false</c>.</returns>
        bool Delete(T obj);


        /// <summary>
        /// Deletes the specified object.
        /// WARNING: Data will be lost and will not be recoverable!
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if object is deleted, else <c>false</c>.</returns>
        Task<bool> DeleteAsync(T obj);

        #endregion Dapper.FastCrud
    }
}