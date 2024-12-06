using System;

namespace InovaBank.Domain.Entities
{
    public class EntityBase
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
