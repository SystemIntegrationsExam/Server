using Server.ApiHelper;
using Server.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Server.Microservices.ReviewService
{
    class Processor
    {
        private Review review = new Review();

        public async Task PostReview()
        {
            using (HttpResponseMessage responseMessage
                = await Apihelper.ApiClient.PostAsJsonAsync("https://sirestreviewfinal.azurewebsites.net/api/values", review)){}
        }

        public Review Review
        {
            get { return review; }
            set { review = value; }
        }
    }
}
