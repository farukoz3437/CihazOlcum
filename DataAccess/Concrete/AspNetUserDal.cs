using Core.DataAccess.EfEntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete
{
    public class AspNetUserDal:EntityRepository<AspNetUser, IMandCRMContext>, IAspNetUserDal
    {
    }
}
