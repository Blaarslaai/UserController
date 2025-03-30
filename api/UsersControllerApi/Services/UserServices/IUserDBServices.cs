using BaseProjectApi.Models;

namespace BaseProjectApi.Services.UserServices
{
    public interface IUserDBServices
    {
        Task<ServiceModel> RegisterUser(UsersModel usrm, UsersProfile usrp);
        Task<ServiceModel> UserLogin(string UserName);
        Task<ServiceModel> GetSingleUser(string UserId);
        Task<ServiceModel> GetAllUsers(SelectionFilterModel payload);
        Task<ServiceModel> UpdateUser(UsersModel usrm);
        Task<ServiceModel> DeleteSingleUser(string UserId);
        Task<ServiceModel> DeleteAllUsers();
    }
}
