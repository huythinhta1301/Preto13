using Newtonsoft.Json.Linq;

namespace Preto13.Model.Response
{
    public class GenericResponse
    {
        public string? message { get; set; }
        public Boolean status { get; set; } = false;
        public string code { get; set; } = "0";
        public JArray data { get; set; }
    }
}
