
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ModernWMS.Core.DI;
using ModernWMS.Core.Models;
namespace ModernWMS.Core.Services
{

    public interface IBaseService<TEntity> : IDependency where TEntity : BaseModel
    {
        
    }
}
