using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AgregatorServer
{
    /// <summary>
    /// Сводное описание для AgServer
    /// </summary>
    [WebService(Namespace = "urn:Agregator", Name = "AgregatorServer", 
        Description ="AgregatorServer provide connect to outside API services")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class AgServer : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Привет всем!";
        }
        [WebMethod]
        public string TestMethod( string name)
        {
            return name;
        }

        [WebMethod]
        public double Convert (double Celsius)
        {
            return ((Celsius * 9) / 5) + 32;
        }
        /*метод GetUser срабатывает в ответ на вход пользователя
         * в систему после регистрации или просто вход*/
        [WebMethod]
        public int GetUser(int id,string firstname, string secondname )
        {
            int code = 1; // режим ответа на вход пользователя в систему
            return code; // 200 - вход разрешен, 403 - вход запрещен
        }
        public class Payment
        {
            public int userId;
            public double cost;
            public int getUserId() { return userId; }
            public double getCost() { return cost; }
            public void setUserId(int id) { userId = id; }
            public void setCost(double inCost) { cost = inCost; }
            public Payment (int id, double Incost)
            {
                userId = id;
                cost = Incost;
            }
            public Payment() { }
        }
        [WebMethod]//need changes
        public Payment outPayment ()
        {
            int id = 1;
            double cost = 10.0;
            Payment outpay = new Payment();
            outpay.userId = id;
            outpay.cost = cost;
            return outpay;
         
        }
        public struct UserRequestLog //структура для выгрузки данных из базы по пользователю
        {
            public int userId;
            public string userRequest;
            public DateTime date;
        }

        [WebMethod]
        public List<UserRequestLog> ListOfUserRequest(string begin, string end)
        {
           UserRequestLog log1 = new UserRequestLog();
            log1.userId = 1;
            log1.userRequest = "ризотто";
            log1.date = DateTime.Now;
            UserRequestLog log2 = new UserRequestLog();
            log2.userId = 1;
            log2.userRequest = "ризотто";
            log2.date = DateTime.Now;
            List<UserRequestLog> listLogo = new List<UserRequestLog>
            {
                log1, log2
            };
            return listLogo;
        }

    }
}
