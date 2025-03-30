using BaseProjectApi.Models;
using BaseProjectApi.Services.EncrytionService;
using Newtonsoft.Json;

namespace BaseProjectApi.Services.UserServices
{
    public class UserServices : IUserServices
    {
        /** Let me say this EXPLICITLY
         * 
         * This Page is used to do all the work
         * - Controller invoks the function and passes the Payload
         * - Service Deserialize the payload into the wanted model
         * - Service then invokes the DataBase Service
         * - The DataBase will only return the requested Data -> no validation needed
         * - Service then computes the result and sends it back to the Controller
         * - Controller Sends back response to User
         * 
         * Please stick to these rules, purpotrators will be repremanded,
         * All validation to be done from sender,
         * 
        **/

        public ServiceModel _result;
        public readonly IUserDBServices _dbu;
        public readonly IEncryptionService _enc;
        public UserServices(IUserDBServices dbu, IEncryptionService enc)
        {
            _dbu = dbu;
            _enc = enc;
            _result = new ServiceModel();


        }
        public async Task<ServiceModel> RegisterUser(RequestModel uireq)
        {
            _result = new ServiceModel();

            try
            {
                var enc_result = new ServiceModel();
                var payload = JsonConvert.DeserializeObject<UsersProfile>(uireq.Payload.ToString());

                enc_result = await _enc.EncryptToken(payload!);
                if (!enc_result.Status) return enc_result;

                var dbModel = new UsersModel()
                {
                    Epoc = payload.UserId.Replace("USR", ""),
                    UserId = payload.UserId,
                    UserName = payload.EmailAddress,
                    UserToken = enc_result.Payload.ToString()!
                };

                _result = await _dbu.RegisterUser(dbModel, payload);


            }
            catch (Exception ex)
            {
                _result.Status = false;
                _result.Code = 500;
                _result.Message = $"RegisterUser(): Exception: {ex}";
            }

            return _result;
        }

        public async Task<ServiceModel> UserLogin(RequestModel uireq)
        {
            var UserData = new ServiceModel();
            _result = new ServiceModel();

            try
            {
                var getdata = JsonConvert.DeserializeObject<UserLoginModel>(uireq.Payload.ToString()!)!;
                UserData = await _dbu.UserLogin(getdata.UserName);

                if (UserData.Payload == null)
                {
                    _result.Status = false;
                    _result.Code = 500;
                    _result.Message = $"LoginUser(): This user does not exist";
                    return _result;
                }

                var payload = UserData.Payload as UsersModel;

                var enc_result = new ServiceModel();

                var decryption = await _enc.DecryptToken(payload);
                if (!decryption.Status) return decryption;

                var usrProfile = JsonConvert.DeserializeObject<UsersProfile>(decryption.Payload.ToString()!);
                var loginDetails = JsonConvert.DeserializeObject<UserLoginModel>(uireq.Payload.ToString());

                if (!usrProfile.Password.Equals(loginDetails.Password))
                {
                    _result.Status = false;
                    _result.Code = 500;
                    _result.Message = $"LoginUser(): Your Password in incorrect";
                    return _result;
                }

                usrProfile.Password = "";
                usrProfile.id = payload.id;

                var LoginResult = new UserLoginResult()
                {
                    Profile = usrProfile,
                    UserToken = payload.UserToken,
                };

                _result.Status = true;
                _result.Code = 200;
                _result.Message = $"LoginUser(): Success you are Approved";
                _result.Payload = LoginResult;

            }
            catch (Exception ex)
            {
                _result.Status = false;
                _result.Code = 500;
                _result.Message = $"LoginUser(): Exception: {ex}";
            }

            return _result;
        }

        public async Task<ServiceModel> GetSingleUser(RequestModel uireq)
        {
            var UserData = new ServiceModel();
            _result = new ServiceModel();

            try
            {
                var getdata = JsonConvert.DeserializeObject<UserLoginModel>(uireq.Payload.ToString()!)!;
                UserData = await _dbu.UserLogin(getdata.UserName);

                if (UserData.Payload == null)
                {
                    _result.Status = false;
                    _result.Code = 500;
                    _result.Message = $"LoginUser(): This user does not exist";
                    return _result;
                }

                var payload = UserData.Payload as UsersModel;

                var enc_result = new ServiceModel();

                var decryption = await _enc.DecryptToken(payload);
                if (!decryption.Status) return decryption;

                var usrProfile = JsonConvert.DeserializeObject<UsersProfile>(decryption.Payload.ToString()!);
                var loginDetails = JsonConvert.DeserializeObject<UserLoginModel>(uireq.Payload.ToString());

                if (!usrProfile.Password.Equals(loginDetails.Password))
                {
                    _result.Status = false;
                    _result.Code = 500;
                    _result.Message = $"LoginUser(): Your Password in incorrect";
                    return _result;
                }

                usrProfile.Password = "";
                usrProfile.id = payload.id;

                var LoginResult = new UserLoginResult()
                {
                    Profile = usrProfile,
                    UserToken = payload.UserToken,
                };

                _result.Status = true;
                _result.Code = 200;
                _result.Message = $"LoginUser(): Success you are Approved";
                _result.Payload = LoginResult;

            }
            catch (Exception ex)
            {
                _result.Status = false;
                _result.Code = 500;
                _result.Message = $"LoginUser(): Exception: {ex}";
            }

            return _result;
        }

        public async Task<ServiceModel> GetAllUsers(RequestModel uireq)
        {
            _result = new ServiceModel();

            try
            {
                var Payload = JsonConvert.DeserializeObject<SelectionFilterModel>(uireq.Payload.ToString()!)!;
                _result = await _dbu.GetAllUsers(Payload);

            }
            catch (Exception ex)
            {
                _result.Status = false;
                _result.Code = 500;
                _result.Message = $"GetAllUsers(): Exception: {ex}";
            }

            return _result;
        }

        public async Task<ServiceModel> UpdateUser(RequestModel uireq)
        {
            _result = new ServiceModel();

            try
            {
                var enc_result = new ServiceModel();
                var payload = JsonConvert.DeserializeObject<UsersProfile>(uireq.Payload.ToString());

                enc_result = await _enc.EncryptToken(payload!);
                if (!enc_result.Status) return enc_result;

                var dbModel = new UsersModel()
                {
                    Epoc = payload.UserId.Replace("USR", ""),
                    UserId = payload.UserId,
                    UserName = payload.EmailAddress,
                    UserToken = enc_result.Payload.ToString()!
                };

                _result = await _dbu.UpdateUser(dbModel);

            }
            catch (Exception ex)
            {
                _result.Status = false;
                _result.Code = 500;
                _result.Message = $"RegisterUser(): Exception: {ex}";
            }

            return _result;
        }
        public async Task<ServiceModel> DeleteSingleUser(RequestModel uireq)
        {
            var UserData = new ServiceModel();
            _result = new ServiceModel();

            try
            {
                var Payload = JsonConvert.DeserializeObject<UsersModel>(uireq.Payload.ToString()!)!;
                _result = await _dbu.DeleteSingleUser(Payload.UserId);

            }
            catch (Exception ex)
            {
                _result.Status = false;
                _result.Code = 500;
                _result.Message = $"DeleteSingleUser(): Exception: {ex}";
            }

            return _result;
        }

        public async Task<ServiceModel> DeleteAllUsers(RequestModel uireq)
        {
            _result = new ServiceModel();

            try
            {
                _result = await _dbu.DeleteAllUsers();

            }
            catch (Exception ex)
            {
                _result.Status = false;
                _result.Code = 500;
                _result.Message = $"LoginUser(): Exception: {ex}";
            }

            return _result;
        }

        public async Task<ServiceModel> DecryptUserToken(RequestModel uireq)
        {
            var UserData = new ServiceModel();
            _result = new ServiceModel();

            try
            {
                var Payload = JsonConvert.DeserializeObject<UsersModel>(uireq.Payload.ToString()!)!;

                var enc_result = new ServiceModel();

                var decryption = await _enc.DecryptToken(Payload);
                if (!decryption.Status) return decryption;

                var usrProfile = JsonConvert.DeserializeObject<UsersProfile>(decryption.Payload.ToString()!);

                _result.Status = true;
                _result.Code = 200;
                _result.Message = $"DecryptUserToken(): Decryption Complete";
                _result.Payload = usrProfile;

            }
            catch (Exception ex)
            {
                _result.Status = false;
                _result.Code = 500;
                _result.Message = $"LoginUser(): Exception: {ex}";
            }

            return _result;
        }

        public async Task<ServiceModel> UserLogout()
        {
            _result = new ServiceModel();

            try
            {
                _result.Status = true;
                _result.Code = 200;
                _result.Message = $"LogoutUser(): Success you are Approved";

            }
            catch (Exception ex)
            {
                _result.Status = false;
                _result.Code = 500;
                _result.Message = $"LogoutUser(): Exception: {ex}";
            }

            return _result;
        }
    }
}
