using ApplicationCore.Entities.Common;
using ApplicationCore.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : BaseEntity, IAggregateRoot
    {
        private readonly string _tableName;
        protected string _connectionString;
        public GenericRepositoryAsync(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tableName = typeof(T).Name;
        }
        public async Task<int> AddAsync(T entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
            var query = $"insert into {_tableName} ({stringOfColumns}) values ({stringOfParameters})";
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var result = await conn.ExecuteAsync(query, entity);
                return result;
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                await conn.ExecuteAsync($"DELETE FROM {_tableName} WHERE [Id] = @Id", new { Id = id });
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var data = await conn.QueryAsync<T>($"SELECT * FROM {_tableName}");
                return data;
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var data = await conn.QueryAsync<T>($"SELECT * FROM {_tableName} WHERE Id = @Id", new { Id = id });
                return data.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<T>> SelectQuery(string where)
        {
            var query = $"select * from {_tableName} ";

            if (!string.IsNullOrWhiteSpace(where))
                query += where;

            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                var data = await conn.QueryAsync<T>(query);
                return data;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
            var query = $"update {_tableName} set {stringOfColumns} where Id = @Id";

            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                await conn.ExecuteAsync(query, entity);
            }
        }

        private IEnumerable<string> GetColumns()
        {
            return typeof(T)
                    .GetProperties()
                    .Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
                    .Select(e => e.Name);
        }
    }
}
