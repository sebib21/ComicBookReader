namespace ComicBookReader.CoreBusiness
{
    public class ComicIssue
    {
        public int IssueId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public byte[] Cover { get; set; }
    }
}
