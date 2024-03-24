﻿

using Application.Errors;

namespace Application.Services.Interfaces
{
    public interface IInviteService
    {
        public Task<IError> InviteEmployee(bool IsManager, int UserId, string Host, String email, bool AsManager);
    }
}