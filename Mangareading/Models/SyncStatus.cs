using System.ComponentModel.DataAnnotations;

public class SyncStatus
{
    [Key]
    public string Key { get; set; }
    
    public string Status { get; set; }
    
    public string Data { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}