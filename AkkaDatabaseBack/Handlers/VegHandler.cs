using Messages;
using Newtonsoft.Json;
using Transport;

namespace AkkaDatabaseBack.Handlers
{
    public class VegHandler : IHandler<AskFindVegetableData>
    {
        private readonly ILogger _logger;

        private readonly List<string> _vegs = new()
        {
            "картошка",
            "морковка",
            "лук",
            "чеснок"
        };

        public async Task Handle(AskFindVegetableData receivedMessage)
        { 
            var found = _vegs.FirstOrDefault(x => x.Contains(receivedMessage.NamePattern.ToLower()));
            var reply = new ReplyFindVegetableData(found != default, string.IsNullOrEmpty(found) ? string.Empty : found);
            Console.WriteLine("RRR:" + JsonConvert.SerializeObject(reply));
        }
    }
}
