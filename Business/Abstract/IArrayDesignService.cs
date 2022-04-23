using Core.Utilities.Result;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IArrayDesignService
    {
        Task<IDataResult<ArrayDesign>> GetById(int arrayDesignId);
        Task<IDataResult<ArrayDesign>> GetByIdKod(string idKod);
        Task<IDataResult<List<ArrayDesign>>> GetList();
        Task<IDataResult<List<ArrayDesignDto>>> GetListArrayDesign();
        Task<IDataResult<ArrayDesign>> Add(ArrayDesign arrayDesign);
        Task<IDataResult<ArrayDesign>> Delete(ArrayDesign arrayDesign);
        Task<IDataResult<ArrayDesign>> Update(ArrayDesign arrayDesign);
    }
}
