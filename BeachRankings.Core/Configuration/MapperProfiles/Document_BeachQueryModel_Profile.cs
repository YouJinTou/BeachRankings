using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using BeachRankings.Core.DAL;
using BeachRankings.Core.Models;

namespace BeachRankings.Core.Configuration.MapperProfiles
{
    public class Document_BeachQueryModel_Profile : Profile
    {
        public Document_BeachQueryModel_Profile()
        {
            var map = this.CreateMap<Document, BeachQueryModel>();

            map.ForMember(d => d.Id, opt => opt.MapFrom((s, d) =>
            {
                if (s["Id"] == BeachPartitionKey.Continent.ToString())
                {
                    return (object)s["Location"];
                }

                return s["Location"].AsString().Split("%")[1];
            }));
        }
    }
}
