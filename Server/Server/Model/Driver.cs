using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Driver
    {
        private string name;
        private string driverlicense;

        public Driver(string name, string driverlicense)
        {
            this.name = name;
            this.driverlicense = driverlicense;
        }

        public string Driverlicense
        {
            get { return driverlicense; }
            set { driverlicense = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}
