using Business.Abstract;
using Business.Constants;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ArrayMeasurementService: IArrayMeasurementService
    {
        private IArrayMeasurementDal _arrayMeasurementDal;
        public ArrayMeasurementService(IArrayMeasurementDal arrayMeasurementDal)
        {
            _arrayMeasurementDal = arrayMeasurementDal;
        }

        public async Task<IDataResult<ArrayMeasurement>> Add(ArrayMeasurement arrayMeasurement)
        {
            arrayMeasurement.IsDelete = false;
            arrayMeasurement.IdKod = HelperMethods.HelperMethods.IdKod();
            ArrayMeasurement addedArrayMeasurement= await _arrayMeasurementDal.Add(arrayMeasurement);
            return new SuccessDataResult<ArrayMeasurement>(message: Messages.ArrayMeasurementAdded, data: addedArrayMeasurement);
        }
        public async Task<IDataResult<ArrayMeasurement>> Delete(ArrayMeasurement arrayMeasurement)
        {
            arrayMeasurement.IsDelete = true;
            arrayMeasurement.DeleteTime = DateTime.Now;
            arrayMeasurement.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _arrayMeasurementDal.Delete(arrayMeasurement);
            return new SuccessDataResult<ArrayMeasurement>(message: Messages.ArrayMeasurementDeleted);
        }
        public async Task<IDataResult<ArrayMeasurement>> Update(ArrayMeasurement arrayMeasurement)
        {
            arrayMeasurement.IsDelete = false;
            await _arrayMeasurementDal.Update(arrayMeasurement);
            return new SuccessDataResult<ArrayMeasurement>(message: Messages.ArrayMeasurementUpdated);
        }
        public async Task<IDataResult<ArrayMeasurement>> GetById(int arrayMeasurementId)
        {
            return new SuccessDataResult<ArrayMeasurement>(await _arrayMeasurementDal.Get(x => x.ArrayMeasurementId == arrayMeasurementId));
        }
        public async Task<IDataResult<ArrayMeasurement>> GetByIdKod(string idKod)
        {
            return new SuccessDataResult<ArrayMeasurement>(await _arrayMeasurementDal.Get(x => x.IdKod == idKod));
        }
        public async Task<IDataResult<List<ArrayMeasurement>>> GetList()
        {
            var value = await _arrayMeasurementDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<ArrayMeasurement>>(value.ToList());
        }
    }
}
