namespace Messages
{
    public class AskFindVegetableData
    {
        public AskFindVegetableData(string namePattern) => NamePattern = namePattern;

        public string NamePattern { get; }
    }
}