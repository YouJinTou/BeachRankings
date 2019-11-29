namespace BR.Core.Events
{
    public class BeachCreated : EventBase
    {
        public BeachCreated(int id, string type, string name)
            : base(id, type)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
