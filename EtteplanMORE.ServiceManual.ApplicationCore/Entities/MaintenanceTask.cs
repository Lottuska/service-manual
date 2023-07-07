using System;
using System.ComponentModel.DataAnnotations;
using EtteplanMORE.ServiceManual.ApplicationCore.Enums;

#nullable enable

namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public class MaintenanceTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The DeviceId field is required.")]
        public int DeviceId { get; set; } // Foreign key

        public DateTime EntryDate { get; set; }

        public string? Description { get; set; }

        public CriticalityLevel CriticalityLevel { get; set; }

        public bool Done { get; set; }


        public FactoryDevice FactoryDevice { get; set; } // Navigation property
    }
}
