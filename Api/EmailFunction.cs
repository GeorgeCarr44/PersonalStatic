using System.Net;
using PersonalStaticApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using Azure;
using MimeKit;
using PersonalStaticApp.Shared.Models;

namespace Api
{
    public class HttpTrigger
    {
        private readonly ILogger _logger;

        public HttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
        }

        //[Function("WeatherForecast")]
        //public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        //{
        //    var randomNumber = new Random();
        //    var temp = 0;

        //    var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = temp = randomNumber.Next(-20, 55),
        //        Summary = GetSummary(temp)
        //    }).ToArray();

        //    var response = req.CreateResponse(HttpStatusCode.OK);
        //    response.WriteAsJsonAsync(result);

        //    return response;
        //}

        [Function("SendEmail")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {

            var email = new MimeMessage();
            //var sendToCVS = Environment.GetEnvironmentVariable("SMTPSendToCSV");
            var sendToCVS = "brandon.dach79@ethereal.email";
            email.From.Add(MailboxAddress.Parse(EmailRequest.Email));
            if (String.IsNullOrEmpty(sendToCVS))
                return req.CreateResponse(HttpStatusCode.BadRequest);
            var sentToList = sendToCVS.Split(',');
            foreach (var sendToAddress in sentToList)
            {
                email.To.Add(MailboxAddress.Parse(sendToAddress));
            }

            email.Subject = $"Contact from {EmailRequest.Name} | {EmailRequest.Email} via TheLemic.co.uk";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = EmailRequest.Body
            };

            using var smtp = new SmtpClient();
            // smtp.Connect(Environment.GetEnvironmentVariable("SMTPHost"), 587, MailKit.Security.SecureSocketOptions.StartTls);
            // smtp.Authenticate(Environment.GetEnvironmentVariable("SMTPUsername"), Environment.GetEnvironmentVariable("SMTPPassword"));
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("brandon.dach79@ethereal.email", "Jbr9HGm5d6mYdb4vsd");
            smtp.Send(email);
            smtp.Disconnect(true);


            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private string GetSummary(int temp)
        {
            var summary = "Mild";

            if (temp >= 32)
            {
                summary = "Hot";
            }
            else if (temp <= 16 && temp > 0)
            {
                summary = "Cold";
            }
            else if (temp <= 0)
            {
                summary = "Freezing";
            }

            return summary;
        }
    }
}
