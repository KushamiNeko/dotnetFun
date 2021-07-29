using TradingReport.Simulation.Dtos;

namespace TradingReport.Simulation.Models
{
    class User
    {
        public string Name { get; init; }

        public string Uid { get; init; }

        public User FromDto(UserDto dto)
        {
            return new User
            {
                Name = dto.Name,
                Uid = dto.Uid
            };
        }
    }
}