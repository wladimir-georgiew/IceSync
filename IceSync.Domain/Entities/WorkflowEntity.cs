using System.ComponentModel.DataAnnotations;

namespace IceSync.Domain.Entities;

public class WorkflowEntity
{
    [Key]
    public int WorkflowId { get; set; }
    public string WorkflowName { get; set; }
    public bool IsActive { get; set; }
    public int MultiExecBehavior { get; set; }
}