using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using BeachRankings.Core.Extensions;
using BeachRankings.Core.Models;

namespace BeachRankings.Core.Configuration.MapperProfiles
{
    public class Document_Beach_Profile : Profile
    {
        public Document_Beach_Profile()
        {
            var map = this.CreateMap<Document, Beach>();

            map.ConstructUsing((s, ctx) =>
            {
                var model = s.ConvertTo<Beach>();
                model.Id = (s["Id"] == Beach.PrimaryPartitionKeyType.ToString()) ?
                    s["Location"].AsString() : s["Location"].AsString().Split("%")[1];

                return model;
            });
        }
    }
}
