// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace PG.Model
{
    public class Facility : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Images { get; set; }
        //public DbGeography Location { get; set; }

        public int SiteId { get; set; }
        public Site Site { get; set; }
    }
}