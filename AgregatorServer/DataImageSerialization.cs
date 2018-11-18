
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace AgregatorServer
{
    [DataContract]
    class DataImageSerialization

    {
        [DataMember]
        public string largeImageURL { get; set; }
        [DataMember]
        public string webformatHeight { get; set; }
        [DataMember]
        public string webformatWidth { get; set; }
        [DataMember]
        public string likes { get; set; }
        [DataMember]
        public string imageWidth { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string user_id { get; set; }
        [DataMember]
        public string views { get; set; }
        [DataMember]
        public string comments { get; set; }
        [DataMember]
        public string pageURL { get; set; }
        [DataMember]
        public string imageHeight { get; set; }
        [DataMember]
        public string webformatURL { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string previewHeight { get; set; }
        [DataMember]
        public string tags { get; set; }
        [DataMember]
        public string downloads { get; set; }
        [DataMember]
        public string user { get; set; }
        [DataMember]
        public string favorites { get; set; }
        [DataMember]
        public string imageSize { get; set; }
        [DataMember]
        public string previewWidth { get; set; }
        [DataMember]
        public string userImageURL { get; set; }
        [DataMember]
        public string previewURL { get; set; }

    }
}