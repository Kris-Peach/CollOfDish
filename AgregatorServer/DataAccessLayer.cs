 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AgregatorServer
{
    class DataAccessLayer
    {
        string connectionString = @"Server=tcp:collofdishesdbserver.database.windows.net,1433;Initial Catalog=CollofDishes;Persist Security Info=False;User ID=valerosha;Password=Server2018;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        /* результат взаимодействия с БД:
           200 - ок
           400 - ошибка добавления 

       */
        public int AddCacheDataInDB(string dishName, string dishDescr, string imageURL)
        {
            int statusResult;
            try
            {
                string sqlExpression = "sp_AddCacheData";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter dishNameParam = new SqlParameter
                    {
                        ParameterName = "@dishName",
                        Value = dishName
                    };
                    command.Parameters.Add(dishNameParam);

                    SqlParameter dishDescrParam = new SqlParameter
                    {
                        ParameterName = "@dishDescription",
                        Value = dishDescr
                    };
                    command.Parameters.Add(dishDescrParam);

                    SqlParameter imageURLParam = new SqlParameter
                    {
                        ParameterName = "@dishImageURL",
                        Value = imageURL
                    };
                    command.Parameters.Add(imageURLParam);

                    var result = command.ExecuteScalar();
                }
                statusResult = 200;
            }

            catch (Exception ex)
            {

                statusResult = 400;

            }
            return statusResult;

        }
        public int AddDishesOrderInDB(int userId, int basketId, double dishCoast, string deliveryAddr, string dateRequest)
        {
            int statusResult;

            try
            {
                string sqlExpression = "sp_AddDishesOrder";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter user_IdParam = new SqlParameter
                    {
                        ParameterName = "@user_Id",
                        Value = userId
                    };
                    command.Parameters.Add(user_IdParam);

                    SqlParameter basket_IdParam = new SqlParameter
                    {
                        ParameterName = "@basket_Id",
                        Value = basketId
                    };
                    command.Parameters.Add(basket_IdParam);

                    SqlParameter dishCoastParam = new SqlParameter
                    {
                        ParameterName = "@dishCoast",
                        Value = dishCoast
                    };
                    command.Parameters.Add(dishCoastParam);


                    SqlParameter deliveryAddrParam = new SqlParameter
                    {
                        ParameterName = "@deliveryAddr",
                        Value = deliveryAddr
                    };
                    command.Parameters.Add(deliveryAddrParam);

                    SqlParameter dateRequestParam = new SqlParameter
                    {
                        ParameterName = "@dateRequest",
                        Value = dateRequest
                    };
                    command.Parameters.Add(dateRequestParam);
                    var result = command.ExecuteScalar();
                }
                statusResult = 200;
            }

            catch (Exception ex)
            {

                statusResult = 400;

            }
            return statusResult;
        }
        public int AddUserReqInDB(int userId, string dishName, string dateRequest)
        {
            int statusResult;

            try
            {
                string sqlExpression = "sp_AddUserReq";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter userIdParam = new SqlParameter
                    {
                        ParameterName = "@client_Id",
                        Value = userId
                    };
                    command.Parameters.Add(userIdParam);

                    SqlParameter dishNameParam = new SqlParameter
                    {
                        ParameterName = "@nameDishRequest",
                        Value = dishName
                    };
                    command.Parameters.Add(dishNameParam);

                    SqlParameter dateRequestParam = new SqlParameter
                    {
                        ParameterName = "@dateRequest",
                        Value = dateRequest
                    };
                    command.Parameters.Add(dateRequestParam);

                    var result = command.ExecuteScalar();
                }
                statusResult = 200;
            }

            catch (Exception ex)
            {

                statusResult = 400;

            }
            return statusResult;
        }
        public DishObject CheckAndRetCacheDB(string dishName)
        {
            DishObject dishObj = new DishObject();
            string sqlExpression = "sp_CheckCache";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter dishNameParam = new SqlParameter
                {
                    ParameterName = "@dishName",
                    Value = dishName
                };
                command.Parameters.Add(dishNameParam);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

                    while (reader.Read())
                    {
                        dishObj.DishName = reader.GetString(0);
                        dishObj.DishDescr = reader.GetString(1);
                        dishObj.ImageURL = reader.GetString(2);
                    }
                }
                else
                {
                    dishObj.DishName = null;
                    dishObj.DishDescr = null;
                    dishObj.ImageURL = null;
                }
                reader.Close();
            }
            return dishObj;
        }
        public List<UserRequest> RetUserRequestDB(string start, string end)
        {
            List<UserRequest> listOfUserReq = new List<UserRequest>();
            string sqlExpression = "sp_RetUserRequest";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter startParam = new SqlParameter
                {
                    ParameterName = "@dateStart",
                    Value = start
                };
                command.Parameters.Add(startParam);
                SqlParameter endParam = new SqlParameter
                {
                    ParameterName = "@dateEnd",
                    Value = end
                };
                command.Parameters.Add(endParam);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

                    while (reader.Read())
                    {
                        UserRequest userRequestObj = new UserRequest();
                        userRequestObj.UserId = reader.GetInt32(1);
                        userRequestObj.DishName = reader.GetString(2);
                        userRequestObj.DateRequest = reader.GetDateTime(3).ToString("yyyy-MM-dd");
                        listOfUserReq.Add(userRequestObj);

                    }
                }

                reader.Close();
            }
            return listOfUserReq;
        }


        public int GetMaxOrderNum ()
        {
            string sqlExpression = "sp_GetLastOrderNum";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter maxOrderNum = new SqlParameter
                {
                    ParameterName = "@request_Id",
                    SqlDbType = System.Data.SqlDbType.Int // тип параметра
                };
                // указываем, что параметр будет выходным
                maxOrderNum.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(maxOrderNum);
                command.ExecuteNonQuery();
                var res = command.Parameters["@request_Id"].Value;
                int result = Convert.ToInt32(command.Parameters["@request_Id"].Value);
                
                return result;
            }
        }
    }
}




