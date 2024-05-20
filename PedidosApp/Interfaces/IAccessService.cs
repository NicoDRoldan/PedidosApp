using Microsoft.AspNetCore.Mvc;

namespace PedidosApp.Interfaces
{
    public interface IAccessService
    {
        Task<string> SendEmailRecover(string emailTo, string codRecover);

        Task<Dictionary<string, object>> ValidateUser(string usuario, string codigoRecuperacion);
    }
}
