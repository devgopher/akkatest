namespace Messages;

public class ReplyFindVegetableData
{
    public ReplyFindVegetableData(bool isSuccess, string name)
    {
        IsSuccess = isSuccess;
        Name = name;
    }

    public bool IsSuccess { get; }
    public string Name { get; }
}