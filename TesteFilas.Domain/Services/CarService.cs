using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Queue.Domain.Interface;
using Queue.Domain.Models;
using Queue.Service.Interfaces;
using Queue.Service.Services;

namespace Queue.Domain.Services
{
    public class CarService : ICarService
    {

        private IApplicationService _applicationService;

        public CarService(IApplicationService application)
        {
            _applicationService = application;
        }
        public async Task<bool> PostCarro(Car modelCar) => await _applicationService.SendData(modelCar, "/novo");
        public async Task<ICollection<Car>> GetCars() => await _applicationService.GetData<Car>();
    }
}
