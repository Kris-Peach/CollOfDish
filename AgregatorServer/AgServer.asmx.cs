using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AgregatorServer.ServiceReference1;

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

        List<User> registUserList = new List<User>();
        List<User> activeUserList = new List<User>();
        DataAccessLayer newConnectDB = new DataAccessLayer();
              


        [WebMethod]
        public DishObject SearchDish(string userSessionId, string dishName)
        {
            int currUserId=0;
            //Связь с БД         
            DishObject dishDef = new DishObject();
            //Ищем пользователя, который сделал запрос
            foreach (User user in activeUserList)
            {
                if (user.UserSessionId == userSessionId)
                {
                    currUserId = user.UserId;
                }
            }
            //Добовляем в таблицу запросов, наш запрос 
            newConnectDB.AddUserReqInDB(currUserId, dishName, DateTime.Now.ToString("yyyy-MM-dd"));
            //Осуществляем поиск блюда в кэше
            dishDef = newConnectDB.CheckAndRetCacheDB(dishName);
            if(dishDef.DishName == null)
            {
                string imageURL = SearchImage(dishName);
                string dishDescr = SearchDescr(dishName);
                dishDef.DishName = dishName;
                dishDef.DishDescr = dishDescr;
                dishDef.ImageURL = imageURL;
                newConnectDB.AddCacheDataInDB(dishName, dishDescr, imageURL);
                return dishDef;
            }
            else return dishDef;  
        }

        //Если listOfUserReq.Count()==0 - ничего по заданному периоду не нашли 
        [WebMethod]
        public List<UserRequest> GetListOfUsersReq(string startDate, string endDate)
        {
            //Связь с БД
            List<UserRequest> listOfUserReq = new List<UserRequest>();
            listOfUserReq = newConnectDB.RetUserRequestDB(startDate, endDate);
            return listOfUserReq;
        }


        //Регистрация нового пользователя
        [WebMethod]
        public int UserRegistration(string userSessionId, string userFirstName, string userSecondName, 
            string userPassword, string userLogin)
        {
            //Связь с Ксюшей
            int result = 1;

            // Если результат != 400, значит нам вернули id нового пользователя и мы создаем нового пользователя
            if(result != 400)
            {
                User newUser = new User(result, userSessionId, userFirstName, userSecondName, userLogin);
                registUserList.Add(newUser);
                return 200;
            }
            else return result;
        }


        //   Проверка корректности ввода данных пользователя при входе в систему 
        [WebMethod]
        public int UserEnter(string userSessionId, string userLogin, string userPassword)
        {
            //Связь с Ксюшей
            int result = 200;

            // Если результат != 400, значит пользователь с такими данными сущ-т=> можем входить в систему
            if (result != 400)
            {
                //Добавляем в список пользователя, который вошел в сеть
                foreach (User user in registUserList)
                {
                    if(user.UserLogin==userLogin)
                    {
                        activeUserList.Add(user);
                    }
                }
                return 200;
            }
            else return result;
        }

        // Возвращает информ пользователя по его  userSessionId 
        [WebMethod]
        public User GetUserInfo(string userSessionId)
        {
            foreach (User user in activeUserList)
            {
                if (user.UserSessionId == userSessionId)
                {
                    return user;

                }
            }
            return null;
        }


        //Поиск корзин
        [WebMethod]
        public List<Cart> СartsSearch(string userSessionId, string dishName, int numberOfCarts)
        {
            //Связь с Настей
            List<Cart> listOfCarts = new List<Cart>();

            return listOfCarts;
        }


        //Создаем заказ
        [WebMethod]
        public string CreateOrder(string userSessionId, int cartId, Address deliveryAddress,
                                                                        double orderCoast, CreditCard cresitCard)
        {
            int currUserId = 0;
            string currUserFirstName = null;
            string deliveryAddr;
            //Отправляем Ксюше запрос, на проверку платежеспособности клиента
            int result = 200;
            if (result != 400)
            {
                //Ищем пользователя, который сделал заказ
                foreach (User user in activeUserList)
                {
                    if (user.UserSessionId == userSessionId)
                    {
                        currUserId = user.UserId;
                        currUserFirstName = user.UserFirstName;
                    }
                }
                deliveryAddr = deliveryAddress.City+" " + deliveryAddress.Street + " " + deliveryAddress.House + " "+deliveryAddress.Apartment;
                //Добавляем заказ в БД

                //Информируем Креню, что заказ был успешно сделан

                //Отправляем запрос мальчикам на доставку

                //deliveryClient.addDelivery(1, currUserId, currUserFirstName, deliveryAddr);
                Order dateAndTypeOrder = new Order();
                newConnectDB.AddDishesOrderInDB(currUserId, cartId, orderCoast, deliveryAddr, dateAndTypeOrder.DeliveryDate);
                return dateAndTypeOrder.DeliveryDate;
            }
            else return "400";
        }


        //Поиск урла картинки
        public string SearchImage(string dishName)
        {
            string searchResult;
            string userToken = "10681699-62b7c4a834f7831db6adff1a6";
            string photoTag = dishName;
            string targetUrl
                = "https://pixabay.com/api/?key=" + userToken + "&q=" + photoTag + "&image_type=photo&lang=ru&page=1&per_page=3";
            //Коннектимся с pixabay
            WebClient myWebClient = new WebClient();
            ServicePointManager.Expect100Continue = false;
            byte[] antwort = null;
            string res;
            try
            {
                antwort = myWebClient.DownloadData(targetUrl);
                res = System.Text.Encoding.ASCII.GetString(antwort);

            }
            catch (Exception ex)
            {
                searchResult = "400";
                return searchResult;
            }

            var jURL = JsonConvert.DeserializeObject<ImageHits>(res);
            if (jURL.hits.Count() == 0)
            {
                return "404";
            }
            else
            {
                searchResult = jURL.hits.First().largeImageURL;
                return searchResult;
            }
        }


        //Поиск описания блюда
        public string SearchDescr(string dishName)
        {
            string searchResult;
            string photoTag = dishName;
            string targetUrl = "https://ru.wikipedia.org/w/api.php?action=opensearch&search=" + photoTag + "&prop=info&format=json&inprop=url";
            //Коннектимся с Wiki
            WebClient myWebClient = new WebClient();
            ServicePointManager.Expect100Continue = false;
            byte[] antwort = null;
            string res;
            try
            {
                antwort = myWebClient.DownloadData(targetUrl);
                res = System.Text.Encoding.GetEncoding("utf-8").GetString(antwort);
            }
            catch (Exception ex)
            {
                searchResult = "400";
                return searchResult;
            }

            JArray ja = JsonConvert.DeserializeObject<JArray>(res);
            List<string[]> strings = new List<string[]>();
            List<JToken> JTokens = new List<JToken>()
                {
                    ja[ja.Count() - 2]
                };

            List<string> ResultDescr = new List<string>();
            for (int i = 0; i < JTokens.Count(); i++)
            {
                JTokens[i].ToList().ForEach(ob => ResultDescr.Add(ob.ToString()));
            }
            searchResult = ResultDescr.First();
            if (searchResult== "")
            {
                return "404";
            }
            else return searchResult;
        }

    }
}
