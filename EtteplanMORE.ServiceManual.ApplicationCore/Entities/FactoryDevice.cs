using System.Collections.Generic;

#nullable enable

namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public class FactoryDevice
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Year { get; set; }

        public string? Type { get; set; }


        public ICollection<MaintenanceTask>? MaintenanceTasks { get; set; }
    }
}