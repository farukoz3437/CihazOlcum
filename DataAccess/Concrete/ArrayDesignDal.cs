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
    public class ArrayDesignDal : EntityRepository<ArrayDesign, IMandCRMContext>, IArrayDesignDal
    {
        public async Task<List<ArrayDesignDto>> GetListArrayDesign()
        {
            using (IMandCRMContext ctx = new IMandCRMContext())
            {
                var query = (from arrayDesign in ctx.ArrayDesigns.Where(x => x.IsDelete == false)
                             join user in ctx.AspNetUsers on arrayDesign.CreatedPersonalId equals user.Id
                             select new ArrayDesignDto
                             {
                                 IdKod = arrayDesign.IdKod,
                                 CreatedPersonalName = user.FirstName + " " + user.LastName,
                                 ArrayCreatedDate = arrayDesign.ArrayCreatedDate,
                                 ArrayName = arrayDesign.ArrayName,
                                 ProjectName=arrayDesign.ProjectName,
                                 CreatedPersonalId = arrayDesign.CreatedPersonalId,
                                 ArrayFormat = arrayDesign.ArrayFormat,
                                 ArrayType = arrayDesign.ArrayType,
                                 CoordinateType = arrayDesign.CoordinateType,
                                 NumberOfStationOneD = arrayDesign.NumberOfStationOneD,
                                 NumberOfStationTwoD = arrayDesign.NumberOfStationTwoD,
                                 GeophoneInterval = arrayDesign.GeophoneInterval,
                                 RotationangleOneD = arrayDesign.RotationangleOneD,
                                 RotationangleTwoD = arrayDesign.RotationangleTwoD,
                                 CenterCoordinateType = arrayDesign.CenterCoordinateType,
                                 Radius = arrayDesign.Radius,
                                 East = arrayDesign.East,
                                 North = arrayDesign.North,
                                 Zone = arrayDesign.Zone,
                                 ZoneCode = arrayDesign.ZoneCode,
                                 Lat = arrayDesign.Lat,
                                 Lon = arrayDesign.Lon
                             });
                var list = await query.ToListAsync();
                return list;
            }
        }
    }
}
