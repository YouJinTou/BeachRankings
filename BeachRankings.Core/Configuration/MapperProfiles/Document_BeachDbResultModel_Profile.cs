using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using BeachRankings.Core.Extensions;
using BeachRankings.Core.Models;

namespace BeachRankings.Core.Configuration.MapperProfiles
{
    public class Document_BeachDbResultModel_Profile : Profile
    {
        public Document_BeachDbResultModel_Profile()
        {
            var map = this.CreateMap<Document, BeachDbResultModel>();

            map.ConstructUsing((s, ctx) =>
            {
                var model = s.ConvertTo<BeachDbResultModel>();
                model.Id = (s["Id"] == Beach.PrimaryPartitionKeyType.ToString()) ?
                    s["Location"].AsString() : s["Location"].AsString().Split("%")[1];

                return model;
            });
        }
    }
}
