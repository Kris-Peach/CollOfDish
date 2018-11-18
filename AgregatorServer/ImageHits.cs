
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;




/*  Json format 
 *  {  
  "totalHits":500,
  "hits":[  
     {  
        "largeImageURL":"https://pixabay.com/get/ea35b50f2ffd063ed1584d05fb1d4796e57fe3d310b80c4090f4c378aee5b4bfd0_1280.jpg",
        "webformatHeight":426,
        "webformatWidth":640,
        "likes":733,
        "imageWidth":6000,
        "id":3040797,
        "user_id":686414,
        "views":370521,
        "comments":104,
        "pageURL":"https://pixabay.com/ru/%D0%BB%D0%B5%D0%B2-%D1%85%D0%B8%D1%89%D0%BD%D0%B8%D0%BA-%D0%BE%D0%BF%D0%B0%D1%81%D0%BD%D0%BE-%D0%B3%D1%80%D0%B8%D0%B2%D0%B0-3040797/",
        "imageHeight":4000,
        "webformatURL":"https://pixabay.com/get/ea35b50f2ffd063ed1584d05fb1d4796e57fe3d310b80c4090f4c378aee5b4bfd0_640.jpg",
        "type":"photo",
        "previewHeight":99,
        "tags":"\u043b\u0435\u0432, \u0445\u0438\u0449\u043d\u0438\u043a, \u043e\u043f\u0430\u0441\u043d\u043e",
        "downloads":268738,
        "user":"Alexas_Fotos",
        "favorites":671,
        "imageSize":7101649,
        "previewWidth":150,
        "userImageURL":"https://cdn.pixabay.com/user/2018/10/29/22-35-50-351_250x250.jpg",
        "previewURL":"https://cdn.pixabay.com/photo/2017/12/26/16/09/lion-3040797_150.jpg"
     }, ....

  ],
  "total":4485
}
*/
namespace AgregatorServer
{
    [DataContract]
    class ImageHits
    {
        [DataMember]
        public string totalHits { get; set; }
        [DataMember]
        public List<DataImageSerialization> hits { get; set; }
        [DataMember]
        public string total { get; set; }
    }
}
