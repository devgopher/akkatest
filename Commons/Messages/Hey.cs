namespace Commons.Messages;

public class Hey
{
    public static Hey Create() => new Hey();

    public Hey()
    {
    }

    public Hey(string thing)
        => Thing = thing;

    public string Thing { get; set; }
}