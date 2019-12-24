namespace Day21.Models
{
    public interface IGiftRegistry
    {
        void Create();

        void Add(string item);

        void Close();
    }
}
