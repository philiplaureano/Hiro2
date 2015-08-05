using System;
using System.Collections.Generic;

namespace Hiro2.Tests.SampleDomains.Vehicles
{
    public class MultiEngineCar : IVehicle
    {
        private IEnumerable<IEngine> _engines;

        public MultiEngineCar(IEnumerable<IEngine> engines)
        {
            _engines = engines;
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}