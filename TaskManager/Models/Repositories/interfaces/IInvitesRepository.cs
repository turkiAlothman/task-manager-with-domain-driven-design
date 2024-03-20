﻿using Microsoft.EntityFrameworkCore;
using TaskManager.Models.DomainModels;

namespace TaskManager.Models.Repositories.interfaces
{
    public interface IInvitesRepository
    {

        public Task<Invites> GetByEmailAndSecretKey(string email, string SecretKey);
        public Task<Invites> GetByEmail(string email);
        public  Task CreateInvite(Invites invit);
        public Task deleteInvite(Invites invite);

    }
}
