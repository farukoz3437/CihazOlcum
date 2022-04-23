﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class ArrayDesignDto
    {
        public string IdKod { get; set; }
        public string CreatedPersonalName { get; set; }
        public DateTime? ArrayCreatedDate { get; set; }
        public string ArrayName { get; set; }
        public string ProjectName { get; set; }
        public string CreatedPersonalId { get; set; }
        public int? ArrayFormat { get; set; }
        public int? ArrayType { get; set; }
        public int? CoordinateType { get; set; }
        public int? NumberOfStationOneD { get; set; }
        public int? NumberOfStationTwoD { get; set; }
        public int? GeophoneInterval { get; set; }//Nokta aralığı
        public int? RotationangleOneD { get; set; } //Döndürme açısı(derece)
        public int? RotationangleTwoD { get; set; } //Döndürme açısı(derece)
        public int? CenterCoordinateType { get; set; }
        public int? Radius { get; set; } //yarı çap

        //UTM parameter
        public double? East { get; set; }
        public double? North { get; set; }
        public int? Zone { get; set; }
        public string ZoneCode { get; set; }

        //D.D. parameter
        public double? Lat { get; set; }
        public double? Lon { get; set; }
    }
}
