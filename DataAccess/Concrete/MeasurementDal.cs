using Core.DataAccess.EfEntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class MeasurementDal : EntityRepository<Measurement, IMandCRMContext>, IMeasurementDal
    {
        public async Task<List<MeasurementDto>> GetListMeasurement()
        {
            using (IMandCRMContext ctx = new IMandCRMContext())
            {
                var query = (from measurement in ctx.Measurements.Where(x => x.IsDelete == false)
                             join user in ctx.AspNetUsers on measurement.UserId equals user.Id
                             select new MeasurementDto
                             {
                                 IdKod = measurement.IdKod,
                                 Description = measurement.Description,
                                 EnviromentBuilding = measurement.EnviromentBuilding,
                                 DeviceName = measurement.DeviceName,
                                 EnviromentBuildingDescription = measurement.EnviromentBuildingDescription,
                                 Frequency = measurement.Frequency,
                                 GroundPlate = measurement.GroundPlate,
                                 GroundPlateDescription = measurement.GroundPlateDescription,
                                 GroundType = measurement.GroundType,
                                 IsRainy = measurement.IsRainy,
                                 IsWindy = measurement.IsWindy,
                                 Lat = measurement.Lat,
                                 Location = measurement.Location,
                                 Lon = measurement.Lon,
                                 Ele = measurement.Ele,
                                 MeasurementDate = measurement.MeasurementDate,
                                 PointName = measurement.PointName,
                                 RainForce = measurement.RainForce,
                                 RecordingTime = measurement.RecordingTime,
                                 ProjectName = measurement.ProjectName,
                                 Sensor = measurement.Sensor,
                                 SensorFrequency = measurement.SensorFrequency,
                                 UserFullName = user.FirstName + " " + user.LastName,
                                 WindForce = measurement.WindForce,
                                 MeasurementImg=measurement.MeasurementImg,
                                 FileE = measurement.FileE,
                                 FileN = measurement.FileN,
                                 FileZ = measurement.FileZ

                             });
                var list = await query.ToListAsync();
                return list;
            }
        }

    }
}
