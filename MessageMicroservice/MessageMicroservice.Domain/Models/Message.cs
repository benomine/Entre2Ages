using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageMicroservice.Domain.Models
{
    [BsonIgnoreExtraElements]
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]  
        public string Id { get; set; }
        [BsonElement("author")]
        public string Author { get; set; }
        [BsonElement("body")]
        public string Body { get; set; }
        [BsonElement("timeStamp")]
        public long TimeStamp { get; set; }
    }
}
