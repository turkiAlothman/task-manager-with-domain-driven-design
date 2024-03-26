using Domain.DTOs;
using Domain.Team;

namespace Domain.DomainModels.Team
{
    public interface ITeamsRepository
    {

        public Task<IEnumerable<Teams>> GetAll();
        public Task<IEnumerable<Teams>> GetAllWithDetails();
        public Task<Teams> GetTeam(int id);
        public Task<int> Count();
        public Task<List<TeamStatus>> GetTeamStatus();
    }
}
