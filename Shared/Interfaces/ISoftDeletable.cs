namespace SharedLibrary.Interfaces
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        DateTime? DeleteOnUtc { get; set; }
    }
}
