using BaseProjectApi.Models;

namespace BaseProjectApi.Services.UserServices
{
    public interface IUserServices
    {
        Task<ServiceModel> RegisterUser(RequestModel uireq);
        Task<ServiceModel> UserLogin(RequestModel uireq);
        Task<ServiceModel> UserLogout();
        Task<ServiceModel> GetSingleUser(RequestModel UserName);
        Task<ServiceModel> GetAllUsers(RequestModel uireq);
        Task<ServiceModel> UpdateUser(RequestModel uireq);
        Task<ServiceModel> DeleteSingleUser(RequestModel uireq);
        Task<ServiceModel> DeleteAllUsers(RequestModel uireq);
        Task<ServiceModel> DecryptUserToken(RequestModel uireq);
    }
}
