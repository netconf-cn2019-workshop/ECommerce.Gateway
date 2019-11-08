using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ECommerce.Gateway.Models.Dtos;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Formatting;

namespace ECommerce.Gateway.Services
{
    public class DataService : IDataService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public DataService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Customer>> GetCustomerAsync(Customer query)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var customersServiceHost = _configuration["Services:Customer"];

            var response = await client.PostAsync($"http://{customersServiceHost}/api/customers/search", query, new JsonMediaTypeFormatter());
            var result = await response.Content.ReadAsAsync <IEnumerable<Customer>>();

            return result;
        }
    }
}




