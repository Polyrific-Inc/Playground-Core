// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace PG.Model
{
    public class Site : BaseModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ICollection<Facility> Facilities { get; set; }
    }
}