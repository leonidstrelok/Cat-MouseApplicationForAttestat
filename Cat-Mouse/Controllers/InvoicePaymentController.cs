using Cat_Mouse.Configuration;
using Cat_Mouse.DbContextModel;
using Cat_Mouse.ModelConfigurations;
using Cat_Mouse.Models;
using Cat_Mouse.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cat_Mouse.Controllers
{
    public class InvoicePaymentController : Controller
    {
        private readonly AddressSite addressSite;
        private readonly AuthorizeConfiguration authorizeData;
        private readonly ApplicationDbContext applicationDb;
        private const string contentType = "application/json";
        public InvoicePaymentController(IOptions<AddressSite> options, IOptions<AuthorizeConfiguration> auhtorizeOptions, ApplicationDbContext applicationDb)
        {
            addressSite = options.Value;
            authorizeData = auhtorizeOptions.Value;
            this.applicationDb = applicationDb;
        }

        public async Task<IActionResult> Index()
        {
            var model = new RegistrationOrderViewModel();
            model.RegistrationOrders = await applicationDb.RegistrationOrders.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> GetStatusOrderById(string orderId)
        {
            var model = new
            {
                userName = authorizeData.UserName,
                password = authorizeData.Password,
                orderId
            };
            using var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(model);
            var httpContent = new StringContent(json, Encoding.UTF8, contentType);
            var res = await httpClient.PostAsync(addressSite.StatusInovicePayment, httpContent);
            var response = await res.Content.ReadAsStringAsync();

            return View();
        }

        public async Task<IActionResult> CreateOrder(RegistrationOrder registrationOrder)
        {
            if (ModelState.IsValid && registrationOrder.Amount > 0)
            {
                registrationOrder.UserName = authorizeData.UserName;
                registrationOrder.Password = authorizeData.Password;
                registrationOrder.OrderNumber = Guid.NewGuid().ToString();
                registrationOrder.ReturnUrl = addressSite.ReturnUrl;
                registrationOrder.FailUrl = addressSite.FailUrl;
                using var httpClient = new HttpClient();
                var json = JsonSerializer.Serialize(registrationOrder);
                var httpContent = new StringContent(json, Encoding.UTF8, contentType);
                var res = await httpClient.PostAsync(addressSite.RegistrationInovicePayment, httpContent);
                var response = await res.Content.ReadAsStringAsync();
                var deserialize = JsonSerializer.Deserialize<ResponseRegistrationOrder>(response);
                if (deserialize != null)
                {
                    if (!string.IsNullOrEmpty(deserialize.formUrl))
                    {
                        registrationOrder.OrderId = deserialize.orderId;
                        await applicationDb.RegistrationOrders.AddAsync(registrationOrder);
                        await applicationDb.SaveChangesAsync();
                        return Redirect(deserialize.formUrl);
                    }
                    else
                    {
                        return Redirect(addressSite.FailUrl);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
