using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMeasurementService
    {
        Task<IDataResult<Measurement>> GetById(int measurementId);
        Task<IDataResult<Measurement>> GetByIdKod(string idKod);
        Task<IDataResult<List<Measurement>>> GetList();
        Task<IDataResult<List<MeasurementDto>>> GetListMeasurementDto();
        Task<IDataResult<Measurement>> Add(Measurement measurement);
        Task<IDataResult<Measurement>> Delete(Measurement measurement);
        Task<IDataResult<Measurement>> Update(Measurement measurement);
    }
}
