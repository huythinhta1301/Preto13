using System.Text.Json;

namespace Preto13.Model
{
    public class ErrorResponse
    {
        
            public int Code { get; set; }
            public string Message { get; set; }
            public override string ToString()
            {
                return JsonSerializer.Serialize(this);
            }
        
    }
}
