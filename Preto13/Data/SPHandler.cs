using Preto13.Config;
using System.Data.SqlClient;
using System.Data;

namespace Preto13.Data
{
    public class SPHandler
    {
        private readonly DatabaseConnection _databaseConnection;

        public SPHandler(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<DataSet> ExecuteStoredProcedure(string storedProcedureName, Dictionary<string, object> parameters)
        {
            using (var connection = _databaseConnection.GetConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        var param = command.CreateParameter();
                        param.ParameterName = parameter.Key;
                        param.Value = parameter.Value;
                        command.Parameters.Add(param);
                    }

                    await connection.OpenAsync();

                    var adapter = new SqlDataAdapter((SqlCommand)command);
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    return dataSet;
                }
            }
        }

    }
}
