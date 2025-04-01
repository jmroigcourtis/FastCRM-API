using CRM.Services.Implementations;
using CRM.Models.Dto;

namespace CRM.Services.Contracts;

public interface IMailService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newUser"></param>
    /// <returns></returns>
    string SendRegisterMail(UserRegDto newUser);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="newPassword"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    string SendNewPasswordMail(string username, string email, string newPassword);
}