using Core.Utilities.Result;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAspNetUserService
    {
        Task<IDataResult<AspNetUser>> GetById(string userId);
        Task<IDataResult<List<AspNetUser>>> GetList();
        Task<IDataResult<AspNetUser>> Add(AspNetUser user);
        Task<IDataResult<AspNetUser>> Delete(AspNetUser user);
        Task<IDataResult<AspNetUser>> Update(AspNetUser user);
    }
}
