using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Gateway.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using ECommerce.Gateway.Models.Dtos;
using System.Net.Http.Headers;
using System.Text;
using System.Security.Claims;

namespace ECommerce.Gateway
{
    public class BasicAuthenticationHandler:AuthenticationHandler<AuthenticationSchemeOptions>
    {

        private IDataService _dataService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IDataService dataService) : base(options, logger, encoder, clock)
        {
            this._dataService = dataService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");


            Customer user = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];

                var customers = await _dataService.GetCustomerAsync(new Customer { UserName = username });
                if (customers == null || !customers.Any())
                    return AuthenticateResult.Fail("Customer Not Found");

                var customer = customers.First();
                if(customer.Password != password)
                    return AuthenticateResult.Fail("Wrong Username Or Password");

                var claims = new List<Claim>() {
                    new Claim("sid",customer.CustomerId.ToString()),
                    new Claim("username", customer.UserName),
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

        }
    }
}
