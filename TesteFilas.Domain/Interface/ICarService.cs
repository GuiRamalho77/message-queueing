using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Queue.Domain.Models;

namespace Queue.Domain.Interface
{
    public interface ICarService
    {
        Task<bool> PostCarro(Car modelCar);
        Task<ICollection<Car>> GetCars();
    }
}
