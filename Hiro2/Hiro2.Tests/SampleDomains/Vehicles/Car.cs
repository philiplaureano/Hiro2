using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiro2.Tests.SampleDomains.Vehicles
{
    public class Car : IVehicle
    {
        private readonly IEngine _engine;

        public Car(IEngine engine)
        {
            _engine = engine;
        }

        public void Start()
        {
            _engine.Start();
        }

        public void Stop()
        {
            _engine.Stop();
        }
    }

    public class CarWithNoEngine : IVehicle
    {
        public void Start()
        {
            throw new NotSupportedException("I have no engine!");
        }

        public void Stop()
        {
            throw new NotSupportedException("I have no engine!");
        }
    }
}
