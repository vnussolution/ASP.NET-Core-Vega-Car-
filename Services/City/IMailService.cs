using System.Diagnostics;

namespace Vega.Services.City {
    public interface IMailService {
        void Send (string subject, string message);
    }
}