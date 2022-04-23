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
    public class ArrayDesignService : IArrayDesignService
    {
        private IArrayDesignDal _arrayDesignDal;
        public ArrayDesignService(IArrayDesignDal arrayDesignDal)
        {
            _arrayDesignDal = arrayDesignDal;
        }

        public async Task<IDataResult<ArrayDesign>> Add(ArrayDesign arrayDesign)
        {
            arrayDesign.ArrayCreatedDate = DateTime.Now;
            arrayDesign.IsDelete = false;
            arrayDesign.IdKod = HelperMethods.HelperMethods.IdKod();
            ArrayDesign addedArrayDesign= await _arrayDesignDal.Add(arrayDesign);
            return new SuccessDataResult<ArrayDesign>(message: Messages.ArrayDesignAdded, data: addedArrayDesign);
        }
        public async Task<IDataResult<ArrayDesign>> Delete(ArrayDesign arrayDesign)
        {
            arrayDesign.IsDelete = true;
            arrayDesign.DeleteTime = DateTime.Now;
            arrayDesign.DeleteIp = HelperMethods.HelperMethods.GetLocalIPAddress();
            await _arrayDesignDal.Delete(arrayDesign);
            return new SuccessDataResult<ArrayDesign>(message: Messages.ArrayDesignDeleted);
        }
        public async Task<IDataResult<ArrayDesign>> Update(ArrayDesign arrayDesign)
        {
            arrayDesign.IsDelete = false;
            await _arrayDesignDal.Update(arrayDesign);
            return new SuccessDataResult<ArrayDesign>(message: Messages.ArrayDesignUpdated);
        }
        public async Task<IDataResult<ArrayDesign>> GetById(int arrayDesignId)
        {
            return new SuccessDataResult<ArrayDesign>(await _arrayDesignDal.Get(x => x.ArrayDesignId == arrayDesignId));
        }
        public async Task<IDataResult<ArrayDesign>> GetByIdKod(string idKod)
        {
            return new SuccessDataResult<ArrayDesign>(await _arrayDesignDal.Get(x => x.IdKod == idKod));
        }
        public async Task<IDataResult<List<ArrayDesign>>> GetList()
        {
            var value = await _arrayDesignDal.GetList(x => x.IsDelete == false);
            return new SuccessDataResult<List<ArrayDesign>>(value.ToList());
        }       
        public async Task<IDataResult<List<ArrayDesignDto>>> GetListArrayDesign()
        {
            var value = await _arrayDesignDal.GetListArrayDesign();
            return new SuccessDataResult<List<ArrayDesignDto>>(value.ToList());
        }

    }
}
