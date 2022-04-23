using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using IMandCRM.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.AutoMapper
{
    public class AutoMapping:Profile
    {
        public AutoMapping()
        {

            CreateMap<Measurement, MeasurementModel>();
            CreateMap<MeasurementModel, Measurement>();

            CreateMap<MeasurementDto, MeasurementModel>();
            CreateMap<MeasurementModel, MeasurementDto>();

            CreateMap<ArrayDesign, CreateArraydesignModel>();
            CreateMap<CreateArraydesignModel, ArrayDesign>();            
            
            CreateMap<ArrayDesignDto, CreateArraydesignModel>();
            CreateMap<CreateArraydesignModel, ArrayDesignDto>();

            CreateMap<ArrayMeasurement, ArrayMeasurementModel>();
            CreateMap<ArrayMeasurementModel, ArrayMeasurement>();
        }
    }
}
