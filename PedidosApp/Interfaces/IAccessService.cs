using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PedidosApp.Interfaces
{
    public interface IAccessService
    {
        Task<string> SendEmailRecover(string emailTo, string codRecover);

        Task<Dictionary<string, object>> ValidateUser(string usuario, string codigoRecuperacion);
        Task<Dictionary<string, object>> ChangePassword(string user, string password);
    }
}
