using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Concrete
{
    public class ArrayMeasurement:CommonFeature,IEntity
    {
        [Key]
        public int ArrayMeasurementId { get; set; }
        public string IdKod { get; set; }
        public string ArrayDesignIdKod { get; set; }
        public string GpsXDesign { get; set; }
        public string GpsYDesign { get; set; }
        public string DeviceName { get; set; }
        public string PointName { get; set; }
        public DateTime? MeasurementDate { get; set; }
        public string Location { get; set; }
        public string UserId { get; set; }
        public int? Frequency { get; set; }
        public int? RecordingTime { get; set; }
        public string Sensor { get; set; }
        public string SensorFrequency { get; set; }
        public string Description { get; set; }
        public bool? IsRainy { get; set; }
        public int? RainForce { get; set; }
        public bool? IsWindy { get; set; }
        public int? WindForce { get; set; }
        public string CartesianX { get; set; }
        public string CartesianY { get; set; }
        public string GpsXField { get; set; }
        public string GpsYField { get; set; }
        public string Ele { get; set; }
        public int? DesignNumber { get; set; }
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
