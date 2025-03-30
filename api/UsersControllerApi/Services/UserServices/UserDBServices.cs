using BaseProjectApi.Models;
using BaseProjectApi.Services.UserService;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace BaseProjectApi.Services.UserServices
{
    public class UserDBServices : IUserDBServices
    {
        public ServiceModel _result;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IDBUsersChecks _dbuc;

        public UserDBServices(IConfiguration configuration,
            IDBUsersChecks dbuc)
        {
            _configuration = configuration;
            _result = new ServiceModel();
            _connectionString = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            _dbuc = dbuc;
        }

        public async Task<ServiceModel> RegisterUser(UsersModel usrm, UsersProfile usrp)
        {
            _result = new ServiceModel();

            try
            {
                /* These are small little checks this may be done in raw query */
                _result = await _dbuc.CheckIfUserIdExist(usrm.UserId);
                if (!_result.Status) return _result;
                _result = await _dbuc.CheckIfUserNameExist(usrm.UserName);
                if (!_result.Status) return _result;

                /* The rest stored procedures here we are submitting data */
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("dbo.InsertUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure
                        command.Parameters.AddWithValue("@UserId", usrm.UserId);
                        command.Parameters.AddWithValue("@UserName", usrm.UserName);
                        command.Parameters.AddWithValue("@UserToken", usrm.UserToken);
                        command.Parameters.AddWithValue("@Epoc", usrm.Epoc);

                        // Open connection and execute command
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                _result.Code = 200;
                _result.Status = true;
                _result.Message = "RegisterUser() User Registered";

            }
            catch (Exception ex)
            {

                _result.Code = 500;
                _result.Status = false;
                _result.Message = "UserLogin() Exception: " + ex.Message;
            }

            return _result;
        }

        public async Task<ServiceModel> UserLogin(string UserName)
        {
            _result = new ServiceModel();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("dbo.GetUserByUserName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserName", UserName); // Ensure this matches the stored procedure parameter

                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var UserModelx = new UsersModel
                                {
                                    id = reader.GetInt32(reader.GetOrdinal("id")), // Adjust this if necessary
                                    UserId = reader.GetString(reader.GetOrdinal("UserId")), // Adjust this if necessary
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                    UserToken = reader.GetString(reader.GetOrdinal("UserToken")),
                                    Epoc = reader.GetString(reader.GetOrdinal("Epoc")),
                                    DateTouched = reader.GetDateTime(reader.GetOrdinal("DateTime"))
                                };

                                _result.Payload = UserModelx;
                            }

                            reader.Close();
                        }

                        command.Clone();
                    }
                }

                _result.Code = 200;
                _result.Status = true;
                _result.Message = "User retrieved successfully"; // Adjust message as needed

            }
            catch (Exception ex)
            {
                _result.Code = 500;
                _result.Status = false;
                _result.Message = "Exception occurred: " + ex.Message;
            }

            return _result;
        }

        public async Task<ServiceModel> GetSingleUser(string UserId)
        {
            _result = new ServiceModel();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("dbo.GetSomeOtherSPHereIFyoucan", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", UserId); // Ensure this matches the stored procedure parameter

                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var UserModelx = new UsersModel
                                {
                                    id = reader.GetInt32(reader.GetOrdinal("id")), // Adjust this if necessary
                                    UserId = reader.GetString(reader.GetOrdinal("UserId")), // Adjust this if necessary
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                    Epoc = reader.GetString(reader.GetOrdinal("Epoc")),
                                    DateTouched = reader.GetDateTime(reader.GetOrdinal("DateTime"))
                                };

                                _result.Payload = UserModelx;
                            }

                            reader.Close();
                        }

                        connection.Close();
                    }
                }

                _result.Code = 200;
                _result.Status = true;
                _result.Message = "GetSingleUser() User retrieved successfully"; // Adjust message as needed

            }
            catch (Exception ex)
            {
                _result.Code = 500;
                _result.Status = false;
                _result.Message = "Exception occurred: " + ex.Message;
            }

            return _result;
        }

        public async Task<ServiceModel> GetAllUsers(SelectionFilterModel payload)
        {
            _result = new ServiceModel();

            try
            {
                /* Here we are reading from the DB via SP and returning a list 
                 * Parsing a model of off-set values to we are returning result 0-100                 
                 */
                var UserList = new List<UsersModel>();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("dbo.GetAllUsers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Offset", payload.OffSetNumber);
                        command.Parameters.AddWithValue("@FetchRows", payload.LimitedNumber);

                        connection.Open();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var user = new UsersModel
                                {
                                    id = reader.GetInt32(reader.GetOrdinal("id")), // Adjust this if necessary
                                    UserId = reader.GetString(reader.GetOrdinal("UserId")), // Adjust this if necessary
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")), // Adjust this if necessary
                                    UserToken = reader.GetString(reader.GetOrdinal("UserToken")),
                                    //Epoc = reader.GetString(reader.GetOrdinal("Epoc")),
                                    DateTouched = reader.GetDateTime(reader.GetOrdinal("DateTime"))
                                };
                                UserList.Add(user);
                            }
                            reader.Close();
                        }
                    }
                }

                /* These are small little checks this may be done in raw query */
                _result = await _dbuc.GetUserTotalCount();
                if (!_result.Status) return _result;

                _result.Code = 200;
                _result.Status = true;
                _result.Message = _result.Payload.ToString()!;
                _result.Payload = UserList;
            }
            catch (Exception ex)
            {

                _result.Code = 500;
                _result.Status = false;
                _result.Message = "GetAllUsers() Exception: " + ex.Message;
            }

            return _result;
        }

        public async Task<ServiceModel> UpdateUser(UsersModel usrm)
        {
            _result = new ServiceModel();

            try
            {
                /* These are small little checks this may be done in raw query */
                _result = await _dbuc.CheckUsernameOnUpdateDuplicate(usrm);
                if (!_result.Status) return _result;

                /* The rest stored procedures here we are submitting data */
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("dbo.UpdateUserByUserId", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@UserId", usrm.UserId);
                        command.Parameters.AddWithValue("@UserName", usrm.UserName);
                        command.Parameters.AddWithValue("@UserToken", usrm.UserToken);
                        command.Parameters.AddWithValue("@Epoc", usrm.Epoc);

                        // Open connection and execute command
                        connection.Open();
                        command.ExecuteNonQuery();

                    }

                    connection.Close();
                }

                _result.Code = 200;
                _result.Status = true;
                _result.Message = "UpdateUser() User Updated";

            }
            catch (Exception ex)
            {

                _result.Code = 500;
                _result.Status = false;
                _result.Message = "UpdateUser() Exception: " + ex.Message;
            }

            return _result;
        }

        public async Task<ServiceModel> DeleteSingleUser(string UserId)
        {
            _result = new ServiceModel();

            try
            {
                /* The rest stored procedures here we are submitting data */
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("dbo.DeleteUserByUsername", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@UserId", UserId);

                        // Open connection and execute command
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }

                _result.Code = 200;
                _result.Status = true;
                _result.Message = "DeleteSingleUser() User Registered";

            }
            catch (Exception ex)
            {

                _result.Code = 500;
                _result.Status = false;
                _result.Message = "DeleteSingleUser() Exception: " + ex.Message;
            }

            return _result;
        }

        public async Task<ServiceModel> DeleteAllUsers()
        {
            _result = new ServiceModel();

            try
            {
                /* The rest stored procedures here we are submitting data */
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("dbo.DeleteAllUsers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Open connection and execute command
                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    connection.Close();

                }

                _result.Code = 200;
                _result.Status = true;
                _result.Message = "DeleteAllUsers() All Users Deleted";

            }
            catch (Exception ex)
            {

                _result.Code = 500;
                _result.Status = false;
                _result.Message = "DeleteAllUsers() Exception: " + ex.Message;
            }

            return _result;
        }
    }
}
