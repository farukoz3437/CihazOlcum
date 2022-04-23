using Core.DataAccess.EfEntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IAspNetUserDal:IEntityRepository<AspNetUser>
    {
    }
}
