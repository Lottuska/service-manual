using System;
using System.ComponentModel.DataAnnotations;
using EtteplanMORE.ServiceManual.ApplicationCore.Enums;

#nullable enable

namespace EtteplanMORE.ServiceManual.ApplicationCore.Dtos
{
    public class MaintenanceTaskDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The DeviceId field is required.")]
        public int DeviceId { get; set; }

        public DateTime EntryDate { get; set; }

        public string? Description { get; set; }

        public CriticalityLevel CriticalityLevel { get; set; }

        public bool Done { get; set; }
    }
}
