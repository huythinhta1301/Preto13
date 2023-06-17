using Newtonsoft.Json.Linq;
using Preto13.Data;
using System.Data;

namespace Preto13.Utils
{
    public class HandleData
    {
        private readonly SPHandler _spHandler;

        public HandleData(SPHandler spHandler)
        {
            _spHandler = spHandler;
        }
        public async Task<JArray> handleSp(string query, Dictionary<string, object> param)
        {
            JArray ja = new JArray();
            try
            {
                DataSet result = await _spHandler.ExecuteStoredProcedure(query, param);

                foreach (DataTable table in result.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        JObject jobj = new JObject();
                        foreach (DataColumn column in table.Columns)
                        {
                            jobj.Add(column.ColumnName, row[column].ToString());
                        }
                        ja.Add(jobj);
                    }
                }
            }
            catch (Exception ex)
            {
                JObject errorObj = new JObject();
                errorObj.Add("error", ex.Message);
                ja.Add(errorObj);
            }
            return ja;
        }
    }
}
