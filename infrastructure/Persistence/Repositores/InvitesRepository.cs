﻿using Microsoft.EntityFrameworkCore;
using Domain.DomainModels.Employee;

namespace infrastructure.Persistence.Repositores
{
    public class InvitesRepository : IInvitesRepository
    {
        private readonly TaskManagerDbContext _context;
        public InvitesRepository(TaskManagerDbContext _context)
        {
            this._context = _context;
        }

        public async Task<Invites> GetByEmailAndSecretKey(string email, string SecretKey)
        {
            return await _context.Invites.Include(i => i.inviter).FirstOrDefaultAsync(i => i.inviteeEmail == email && i.SecretKey == SecretKey);
        }

        public async Task<Invites> GetByEmail(string email)
        {
            return await _context.Invites.FirstOrDefaultAsync(i => i.inviteeEmail == email);
        }
        public async Task CreateInvite(Invites invit)
        {
            await _context.Invites.AddAsync(invit);
            await _context.SaveChangesAsync();
        }

        public async Task deleteInvite(Invites invite)
        {
            _context.Invites.Remove(invite);
            await _context.SaveChangesAsync();
        }



    }
}
