using System.Text.Json.Serialization;

namespace Backend.Dtos;

public class SummarizeResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
    
    [JsonPropertyName("result")]
    public string Result { get; set; }
}