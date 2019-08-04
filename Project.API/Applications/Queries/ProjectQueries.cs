using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace Project.API.Applications.Queries
{
    public class ProjectQueries : IProjectQueries
    {

        private string _connectionString = string.Empty;
        public ProjectQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }
        public async Task<dynamic> GetProejctDetail(int projectId)
        {
            //MySql.Data.MySqlClient.MySqlConnection
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                   @"select *from project where projectid = @projectId"
                        , new { projectId }
                    );

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return result;
            }
        }

        public async  Task<dynamic> GetProjectsByUserId(int userId)
        {
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                   @"select *from project where userid = @userId"
                        , new { userId }
                    );

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return result;
            }
        }
    }
}
