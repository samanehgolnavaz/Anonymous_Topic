using Anonymous_Topics.Services.Intefaces;

namespace Anonymous_Topics.Services.Implementations
{
    public class GuidGenerator : IGuidGenerator
    {
        private string _guid = Guid.NewGuid().ToString();
        public string GetGuid()
        {
            return _guid;
        }
    }
}
