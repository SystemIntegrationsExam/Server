using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Booking
    {
        private Driver driver;
        private Car car;
        private DateTime creation;

        public Booking(Driver driver, Car car, DateTime creation)
        {
            this.driver = driver;
            this.car = car;
            this.creation = creation;
        }

        public DateTime Creation
        {
            get { return DateTime.Now; }
        }

        public Car Car
        {
            get { return car; }
            set { car = value; }
        }

        public Driver Driver
        {
            get { return driver; }
            set { driver = value; }
        }
    }
}
