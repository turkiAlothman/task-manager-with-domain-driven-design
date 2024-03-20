using Microsoft.EntityFrameworkCore;
using TaskManager.Models.DomainModels;
using TaskManager.Models.Repositories.interfaces;

namespace TaskManager.Models.Repositories.implementers
{
    public class TeamsRepository : ITeamsRepository
    {
        private readonly TaskManagerDbContext _context;
        public TeamsRepository(TaskManagerDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Teams>> GetAll()
        {
            return await _context.teams.ToListAsync();
        }
        public async Task<IEnumerable<Teams>> GetAllWithDetails()
        {
            return await _context.teams.Include(t=>t.Members).ToListAsync();
        }
        public async Task<Teams> GetTeam(int id)
        {
            return await _context.teams.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
