using System.Collections.Generic;

namespace Day21.Models
{
    public class GiftRegistry : IGiftRegistry
    {
        public List<string> Items { get; set; } = new List<string>();

        public bool IsOpen { get; set; }

        public void Create() => IsOpen = true;

        public void Add(string item) => Items.Add(item);

        public void Close() => IsOpen = false;
    }
}
