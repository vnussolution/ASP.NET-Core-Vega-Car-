using System.Diagnostics;

namespace Vega.Services.City {
    public class LocalMailService : IMailService {
        private string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send (string subject, string message) {

            Debug.WriteLine ($" Mail from {_mailFrom} to {_mailTo}  , with LocalMailService");
            Debug.WriteLine ($" Subject: {subject}");
            Debug.WriteLine ($" Message: {message}");
        }
    }
}