using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Car
    {
        private string type;
        private string date;
        private string color;
        private int price;
        //private int hk;

        public Car()
        {

        }

        public Car(string type, string date, string color, int price)
        {
            this.type = type;
            this.date = date;
            this.color = color;
            this.price = price;
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Date
        {
            get { return date; }
            set { date = value; }
        } 
        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public override string ToString()
        {
            return $"{type} {color} {price} {date} ";
        }
    }
}
