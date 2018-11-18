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
        public DishObject SearchDish(string dishName)
        {
            //Связь с БД
            DataAccessLayer newConnectDB = new DataAccessLayer();            
            DishObject dishDef = new DishObject();
            //Добовляем в таблицу запросов, наш запрос !!!!!!!!надо вместо 1-го параметра поставить Id текущего пользователя
            newConnectDB.AddUserReqInDB(1, dishName, DateTime.Now.ToString("yyyy-MM-dd"));
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
            DataAccessLayer newConnectDB = new DataAccessLayer();
            List<UserRequest> listOfUserReq = new List<UserRequest>();
            listOfUserReq = newConnectDB.RetUserRequestDB(startDate, endDate);
            return listOfUserReq;
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
            searchResult = jURL.hits.First().largeImageURL;
            return searchResult;
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
            return searchResult;
        }

    }
}
