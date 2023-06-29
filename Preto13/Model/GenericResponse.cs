using Newtonsoft.Json.Linq;

namespace Preto13.Model
{
    public class GenericResponse
    {
        public string? message { get; set; }
        public bool status { get; set; } = false;
        public string code { get; set; } = "0";
        public JArray? data { get; set; }
    }
}
