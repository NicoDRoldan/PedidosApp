using Microsoft.AspNetCore.Mvc;

namespace PedidosApp.Interfaces
{
    public interface IAccessService
    {
        Task<string> SendEmailRecover(string emailTo, string codRecover);
    }
}
