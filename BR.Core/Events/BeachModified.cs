namespace BR.Core.Events
{
    public class BeachModified : EventBase
    {
        public BeachModified(int id, string type) 
            : base(id, type)
        {
        }
    }
}
