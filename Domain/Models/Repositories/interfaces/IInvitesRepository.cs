using Domain.Employee;

namespace Domain.Models.Repositories.interfaces
{
    public interface IInvitesRepository
    {

        public Task<Invites> GetByEmailAndSecretKey(string email, string SecretKey);
        public Task<Invites> GetByEmail(string email);
        public System.Threading.Tasks.Task CreateInvite(Invites invit);
        public System.Threading.Tasks.Task deleteInvite(Invites invite);

    }
}
