using System.Collections.Generic;
using System.Threading.Tasks;

namespace Queue.Service.Interfaces
{
    public interface IApplicationService
    {
        Task<bool> SendData<T>(T model, string endPointSuffix = null) where T : class;
        Task<ICollection<T>> GetData<T>() where T : class;
    }
}
