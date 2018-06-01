// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using PG.Api.Domains.Base;

namespace PG.Api.Domains.Facility
{
    public class NewFacilityDto : BaseNewDto<Model.Facility>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Images { get; set; }
        //public DbGeography Location { get; set; }
    }
}