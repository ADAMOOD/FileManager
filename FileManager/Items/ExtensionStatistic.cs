namespace FileManager
{
    public class ExtensionStatistic
    {
        public string Name { get; set; }
        public long Count { get; set; }
        public long Sum { get; set; }

        public ExtensionStatistic(string name, long count, long sum)
        {
            Name = name;
            Count = count;
            Sum = sum;
        }
    }
}