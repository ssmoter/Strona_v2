using HashidsNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strona_v2.Shared.File
{
    public class TagModel
    {
        public string? Value { get; set; }

    }

    public class TagModelClient : TagModel
    {
        public string? Id { get; set; }
        public string? FileId { get; set; }
        public string? UserId { get; set; }
        public TagModelClient()
        { }
        public TagModelClient(TagModelServer server, IHashids hashids)
        {
            Id = hashids.Encode(server.Id);
            FileId = hashids.Encode(server.FileId);
            UserId = hashids.Encode(server.UserId);
            Value = server.Value;
        }
    }

    public class TagModelServer : TagModel
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public TagModelServer()
        { }
        public TagModelServer(TagModelClient client, IHashids hashids)
        {
            Id = hashids.Decode(client.Id)[0];
            FileId = hashids.Decode(client.FileId)[0];
            UserId = hashids.Decode(client.UserId)[0];
            Value = client.Value;
        }
    }
}
