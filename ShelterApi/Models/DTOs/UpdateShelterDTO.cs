namespace ShelterApi.Models.DTOs
{
    public class UpdateShelterDTO
    {
        public string Name { get; set; } = null!;

        public string City { get; set; } = null!;

        public int Capacity { get; set; }
    }
}
