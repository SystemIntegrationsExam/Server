using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Review
    {
        private int rating;
        private string description;
        private string location;
        private string gender;
        private int age;

        public Review()
        {

        }
        
        public int Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

    }
}
