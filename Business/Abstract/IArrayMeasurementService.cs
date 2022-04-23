using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IArrayMeasurementService
    {
        Task<IDataResult<ArrayMeasurement>> GetById(int arrayMeasurementId);
        Task<IDataResult<ArrayMeasurement>> GetByIdKod(string idKod);
        Task<IDataResult<List<ArrayMeasurement>>> GetList();
        Task<IDataResult<ArrayMeasurement>> Add(ArrayMeasurement arrayMeasurement);
        Task<IDataResult<ArrayMeasurement>> Delete(ArrayMeasurement arrayMeasurement);
        Task<IDataResult<ArrayMeasurement>> Update(ArrayMeasurement arrayMeasurement);
    }
}
