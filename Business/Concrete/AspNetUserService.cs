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
    public class AspNetUserService: IAspNetUserService
    {
        private IAspNetUserDal _aspNetUserDal;
        public AspNetUserService(IAspNetUserDal aspNetUserDal)
        {
            _aspNetUserDal = aspNetUserDal;
        }

        public async Task<IDataResult<AspNetUser>> Add(AspNetUser user)
        {
            AspNetUser addedUserawait = await _aspNetUserDal.Add(user);
            return new SuccessDataResult<AspNetUser>(message: Messages.UserAdded, data: addedUserawait);
        }
        public async Task<IDataResult<AspNetUser>> Delete(AspNetUser user)
        {
            user.IsDelete = true;
            await _aspNetUserDal.Delete(user);
            return new SuccessDataResult<AspNetUser>(message: Messages.UserDeleted);
        }
        public async Task<IDataResult<AspNetUser>> Update(AspNetUser user)
        {
            await _aspNetUserDal.Update(user);
            return new SuccessDataResult<AspNetUser>(message: Messages.UserUpdated);
        }
        public async Task<IDataResult<AspNetUser>> GetById(string userId)
        {
            return new SuccessDataResult<AspNetUser>(await _aspNetUserDal.Get(x => x.Id == userId));
        }
        public async Task<IDataResult<List<AspNetUser>>> GetList()
        {
            var value = await _aspNetUserDal.GetList(x=>x.IsDelete==false);
            return new SuccessDataResult<List<AspNetUser>>(value.ToList());
        }
    }
}
