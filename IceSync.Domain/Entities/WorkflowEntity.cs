using System.ComponentModel.DataAnnotations;

namespace IceSync.Domain.Entities;

public class WorkflowEntity
{
    [Key]
    public int Id { get; set; }
    public int WorkflowId { get; set; }
    public string WorkflowName { get; set; }
    public bool IsActive { get; set; }
    public string MultiExecBehavior { get; set; }
}