using System;

namespace Hiro2.Tests.SampleDomains.Vehicles
{
    public class StockEngine : IEngine
    {
        public void Start()
        {
            Console.WriteLine("Engine started");
        }

        public void Stop()
        {
            Console.WriteLine("Engine stopped");
        }
    }
}