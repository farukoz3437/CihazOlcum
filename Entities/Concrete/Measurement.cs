using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class Measurement: CommonFeature,IEntity
    {
        [Key]
        public int MeasurementId { get; set; }
        public string IdKod { get; set; }
        public string DeviceName { get; set; }
        public string PointName { get; set; }
        public DateTime MeasurementDate { get; set; }
        public string Location { get; set; }
        public string UserId { get; set; }
        public int? Frequency { get; set; }
        public int? RecordingTime { get; set; }
        public string ProjectName { get; set; }
        public string Sensor { get; set; }
        public string SensorFrequency { get; set; }
        public string Description { get; set; }
        public bool? IsRainy { get; set; }
        public int? RainForce { get; set; }
        public bool? IsWindy { get; set; }
        public int? WindForce { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Ele { get; set; }
        public int? GroundType { get; set; }
        public int? GroundPlate { get; set; }
        public string GroundPlateDescription { get; set; }
        public int? EnviromentBuilding { get; set; }
        public string EnviromentBuildingDescription { get; set; }
        public string MeasurementImg { get; set; }
        public string FileE { get; set; }
        public string FileN { get; set; }
        public string FileZ { get; set; }

    }
}
