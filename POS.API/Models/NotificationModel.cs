using FirebaseAdmin.Messaging;
using Newtonsoft.Json;

namespace BTTEM.API.Models
{
    public class NotificationMessage
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("isAndroiodDevice")]
        public bool IsAndroiodDevice { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("Type")]
        public string Type { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("IsAndroid")]
        public string IsAndroid { get; set; }
       
    }

    public class GoogleNotification
    {
        public class DataPayload
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("body")]
            public string Body { get; set; }
            [JsonProperty("Type")]
            public string Type { get; set; }
            [JsonProperty("Id")]
            public string Id { get; set; }          
        }

        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";

        [JsonProperty("data")]
        public DataPayload Data { get; set; }

        [JsonProperty("notification")]
        public DataPayload Notification { get; set; }
    }


    public class Android
    {
        
        [JsonProperty("priority")]
        public string Priority { get; set; }
    }

    public class Message
    {        
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("notification")]
        public Notification Notification { get; set; }
        [JsonProperty("android")]
        public Android Android { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Notification
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
    }
    public class Data
    {
        //[JsonProperty("createdByUserId")]
        //public string CreatedByUserId { get; set; }
        //[JsonProperty("messageType")]
        //public string MessageType { get; set; }
        //[JsonProperty("id")]
        //public string Id { get; set; }

        [JsonProperty("notificationTitle")]
        public string NotificationTitle { get; set; }
        [JsonProperty("notificationBody")]
        public string NotificationBody { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("screen")]
        public string Screen { get; set; }
        [JsonProperty("customKey")]
        public string CustomKey { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class NotificationModel
    {
        [JsonProperty("message")]
        public Message Message { get; set; }
    }
}
