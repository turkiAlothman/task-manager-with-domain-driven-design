using Domain.Models.DomainModels;

namespace Domain.Models.Repositories.interfaces
{
    public interface ITeamsRepository
    {

        public Task<IEnumerable<Teams>> GetAll();
        public Task<IEnumerable<Teams>> GetAllWithDetails();
        public Task<Teams> GetTeam(int id);
    }
}
