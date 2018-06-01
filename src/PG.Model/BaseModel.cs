using System;

namespace PG.Model
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
