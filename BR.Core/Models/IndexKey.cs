using System.Linq;

namespace BR.Core.Models
{
    public class IndexKey
    {
        public IndexKey(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            var isPrimaryKey = id.Contains("|");

            if (isPrimaryKey)
            {
                this.Bucket = id.Split('|')[0];
                this.Token = id.Split('|')[1];
            }
            else
            {
                this.Bucket = string.Join(string.Empty, id.Take(2));
                this.Token = id.ToLower();
            }
        }

        public string Bucket { get; }

        public string Token { get; }
    }
}
