namespace Hackathon2024API.Data.DTO
{
    public record PaginationDTO
    {
        public int Count { get; set; }
        public int Page { get; set; }
    }
}
