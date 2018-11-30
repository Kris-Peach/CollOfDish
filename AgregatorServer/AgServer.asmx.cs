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
using AgregatorServer.ServiceReference2;
using AgregatorServer.ServiceReference3;

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
        DeliveryServiceClient deliveryClient = new DeliveryServiceClient("BasicHttpBinding_IDeliveryService");
        AgregatorServer.ServiceReference3.LibraryClient orderConfirmClient = new AgregatorServer.ServiceReference3.LibraryClient();
        AgregatorServer.ServiceReference2.LibraryClient orderCartsClient = new AgregatorServer.ServiceReference2.LibraryClient();
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
            //orderConfirmClient.
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

            User newUser = new User();
            newUser.UserId = 1;
            newUser.UserFirstName = "Lera";
            newUser.UserSecondName = "Ia ustal";
            newUser.UserLogin = "vse besit";
            newUser.UserSessionId = userSessionId;
            return newUser;
        }



        // Возвращает новый номер заказа по  userSessionId 
        [WebMethod]
        public string generateOrderId(string userSessionId)
        {
          
            string orderNum = null;
            Random rnd = new Random();
            //Получить случайное число (в диапазоне от 1 до 5)
            int sizeOfOrderNum = rnd.Next(5, 7);
            orderNum = RandomString(sizeOfOrderNum);

            return orderNum;
    
        }

        private Random _random = new Random(Environment.TickCount);
        public string RandomString(int length)
        {
            string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }

        //Поиск корзин
        [WebMethod]
        public List<Cart> СartsSearch(string userSessionId, string dishName, int numDish , string orderId)
        {
            //Связь с Настей
            List<Cart> testListOfCarts = new List<Cart>();
            AgregatorServer.ServiceReference2.order newOrderN = new AgregatorServer.ServiceReference2.order();
            newOrderN.order_id = orderId;
            newOrderN.number_of_servings = numDish.ToString();
            newOrderN.dish_name = dishName;
            cart_list newListOfCarts = new cart_list();
            //получаем долгожданный список корзин
            newListOfCarts = orderCartsClient.bookYear(newOrderN);
            int count = newListOfCarts.carts.Count<cart>();
            //Добовляем в свою структуру данных
            List<Cart> listOfCarts = new List<Cart>();

            //если корзин нет
            if (newListOfCarts.carts.Count<cart>() == 0)
            {
                return null;
            }

            else
            {
                foreach (cart newGetCart in newListOfCarts.carts)
                {
                    Cart cartNew = new Cart();
                    cartNew.ProductList = new List<Product>();
                    cartNew.CartId = Convert.ToInt32(newGetCart.cart_id);

                    cartNew.Date = newGetCart.date;
                    cartNew.TotalPrice = Convert.ToInt32(newGetCart.total_price);
                    foreach (product newProduct in newGetCart.product_list)
                    {
                        Product productNew = new Product();
                        productNew.Name = newProduct.name;
                        productNew.Price = Convert.ToInt32(newProduct.price);
                        productNew.Weight = Convert.ToInt32(newProduct.weigth);
                        cartNew.ProductList.Add(productNew);
                    }
                    listOfCarts.Add(cartNew);
                }

                return listOfCarts;
            }
        }



        //Создаем заказ
        [WebMethod]
         public string CreateOrder(string userSessionId, int cartId, Address deliveryAddress,
                                                                      double orderCoast, CreditCard creditCard, string orderId )
      // public string CreateOrder(string userSessionId, int cartId,  double orderCoast, int orderId , string deliveryAddr)
        {
            int currUserId = 0;
            string currUserFirstName = null;
            string deliveryAddr;

            //Ищем пользователя, который сделал заказ
                foreach (User user in activeUserList)
                {
                    if (user.UserSessionId == userSessionId)
                    {
                        currUserId = user.UserId;
                        currUserFirstName = user.UserFirstName;
                    }
                }
            //Отправляем Ксюше запрос, на проверку платежеспособности клиента


            
            int result = 200;
            if (result != 400)
            {
                 deliveryAddr = deliveryAddress.City + " " + deliveryAddress.Street + " " + deliveryAddress.House + " " + deliveryAddress.Apartment;

                //Информируем Креню, что заказ был успешно сделан
                descision newClientDescision = new descision();
                descision2 newClientDescision2 = new descision2();
                newClientDescision.cart_id = cartId.ToString();
                newClientDescision.order_id = orderId;
                newClientDescision2 = orderConfirmClient.validation(newClientDescision);

                if (newClientDescision2.status == "OK")
                   {
                        //Отправляем запрос мальчикам на доставку
                        DeliveryInfo newDeliveryInf = new DeliveryInfo();
                        //newDeliveryInf = deliveryClient.addDelivery(88, 5, "lera", deliveryAddr);
                        newDeliveryInf = deliveryClient.addDelivery(orderId, currUserId, currUserFirstName, deliveryAddr);
                        if (newDeliveryInf.IsReady)
                        {
                            Order dateAndTypeOrder = new Order();
                            dateAndTypeOrder.DeliveryType = newDeliveryInf.Delivery_type;
                            dateAndTypeOrder.DeliveryDate = newDeliveryInf.Time.ToString("dd/MM/yyyy");
                            //Добавляем заказ в БД  
                            newConnectDB.AddDishesOrderInDB(currUserId, cartId, orderCoast, deliveryAddr, dateAndTypeOrder.DeliveryDate);


                            return dateAndTypeOrder.DeliveryDate;
                        }
                    else return "400";
                }
                else return "400";
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
                searchResult = null;
                return searchResult;
            }

            var jURL = JsonConvert.DeserializeObject<ImageHits>(res);
            if (jURL.hits.Count() == 0)
            {
                return null;
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
            if (ResultDescr.Count() == 0)
            {
                return "Определение не найдено";
            }
            else
            {
                searchResult = ResultDescr.First();
                if (searchResult == "")
                {
                    return "Определение не найдено";
                }
                else return searchResult;

            }

        }

    }
}


