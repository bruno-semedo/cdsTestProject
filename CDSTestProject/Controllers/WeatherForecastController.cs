using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace CDSTestProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

            ServiceClient serviceClient = new ServiceClient("AuthType=OAuth;Username=BrunoSemedo@VangenPlotz641.onmicrosoft.com; Password=Fiodental37!; Url=https://org65683223.crm4.dynamics.com/;");

            //ServiceClient serviceClientATSkog = new ServiceClient("AuthType=OAuth;Username=vangenplotz@atskog.no; Password=5CoreCokeReferen$e1!; Url=https://atskog.atskog.no;");


            var data = serviceClient.GetAllEntityMetadata();

            //FetchQueries Xml schema : https://docs.microsoft.com/en-us/powerapps/developer/data-platform/fetchxml-schema

            //Read operation
            var entity1 = serviceClient.GetEntityDataByFetchSearch(@"
                <fetch mapping='logical'>  
                    <entity name='account'>  
                        <attribute name='name' alias='name'/> 
                        <attribute name='creditonhold' alias='creditonhold'/> 
                        <attribute name='lastonholdtime' alias='lastonholdtime'/> 
                        <attribute name='address1_latitude' alias='address1_latitude'/> 
                        <attribute name='address1_longitude' alias='address1_longitude'/> 
                    </entity>
                </fetch> 
            ");

            //Create operation
            var newAccount = new Entity("account");

            newAccount["name"] = "TestAccountCreation";
            newAccount["creditonhold"] = true;
            // DateTime
            newAccount["lastonholdtime"] = DateTime.Now;
            // Double
            newAccount["address1_latitude"] = 47.642311;
            newAccount["address1_longitude"] = -122.136841;
            // Int
            newAccount["numberofemployees"] = 400;
            // Money
            newAccount["revenue"] = new Money(new Decimal(2000000.00));
            // Picklist (Option set)
            newAccount["accountcategorycode"] = new OptionSetValue(2); //Standard customer


            var newLead = new Entity("lead");
            newLead["fullname"] = "Test lead";
            newLead["firstname"] = "Test";
            newLead["lastname"] = "lead";
            newLead["subject"] = "Testing lead creation";
            newLead["statuscode"] = 1;

            serviceClient.Create(newLead);
            

            //Update operation
            var retrievedAccount = new Entity("account", new Guid("88cea450-cb0c-ea11-a813-000d3a1b1223"));

            //Use Entity class with entity logical name
            var account = new Entity("account");
            account.Id = retrievedAccount.Id;
            // set attribute values
            // Boolean (Two option)
            account["name"] = "Test Name Change";
            account["creditonhold"] = true;
            // DateTime
            account["lastonholdtime"] = DateTime.Now;
            // Double
            account["address1_latitude"] = 47.642311;
            account["address1_longitude"] = -122.136841;
            // Int
            account["numberofemployees"] = 400;
            // Money
            account["revenue"] = new Money(new Decimal(2000000.00));
            // Picklist (Option set)
            account["accountcategorycode"] = new OptionSetValue(2); //Standard customer

            //Update the account
            serviceClient.Update(account);



            //Delete operation
            serviceClient.Delete("account", Guid.Parse("61de6985-784b-ec11-8c62-000d3ade8966"));



            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
