using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MeasurementService : IMeasurementService
    {
        private IMeasurementDal _measurementDal;
        public MeasurementService(IMeasurementDal measurementDal)
        {
            _measurementDal = measurementDal;
        }

        public async Task<IDataResult<Measurement>> Add(Measurement measurement)
        {
            measurement.MeasurementDate = DateTime.Now;
            measurement.IsDelete = false;
            measurement.IdKod = HelperMethods.HelperMethods.IdKod();
            Measurement addedMeasurement = await _measurementDal.Add(measurement);
            return new SuccessDataResult<Measurement>(message: Messages.MeasurementAdded, data: addedMeasurement);
        }
        public async Task<IDataResult<Measurement>> Delete(Measurement measurement)
        {
            measurement.IsDelete = true;
            measurement.DeleteTime = DateTime.Now;
            measurement.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _measurementDal.Delete(measurement);
            return new SuccessDataResult<Measurement>(message: Messages.MeasurementDeleted);
        }
        public async Task<IDataResult<Measurement>> Update(Measurement measurement)
        {
            measurement.IsDelete = false;
            await _measurementDal.Update(measurement);
            return new SuccessDataResult<Measurement>(message: Messages.MeasurementUpdated);
        }
        public async Task<IDataResult<Measurement>> GetById(int measurementId)
        {
            return new SuccessDataResult<Measurement>(await _measurementDal.Get(x => x.MeasurementId == measurementId));
        }
        public async Task<IDataResult<Measurement>> GetByIdKod(string idKod)
        {
            return new SuccessDataResult<Measurement>(await _measurementDal.Get(x => x.IdKod == idKod));
        }
        public async Task<IDataResult<List<Measurement>>> GetList()
        {
            var value = await _measurementDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<Measurement>>(value.ToList());
        }

        public async Task<IDataResult<List<MeasurementDto>>> GetListMeasurementDto()
        {
            var value = await _measurementDal.GetListMeasurement();
            return new SuccessDataResult<List<MeasurementDto>>(value.ToList());
        }
    }
}
