public enum CommandType
{
    Move,
    Rotate
}
public interface ICommand
{
    CommandType commandType { get; set; }
    void Execute(Player player);
}
