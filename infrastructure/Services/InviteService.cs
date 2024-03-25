using Application.Errors;
using Application.Errors.Authorizations;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models.Repositories.interfaces;
using infrastructure.Extentions;
using infrastructure.Persistence.Repositores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomString4Net;
using Application.Errors.Invites;
using Domain.Base;

namespace infrastructure.Services
{
    public class InviteService : BaseService, IInviteService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IInvitesRepository _invitesRepository;
        private readonly IEmailService _emailService;

        public InviteService(IUnitOfWork unitOfWork ,IEmployeesRepository employeesRepository, IInvitesRepository invitesRepository, IEmailService emailService) : base(unitOfWork)
        {
            _employeesRepository = employeesRepository;
            _invitesRepository = invitesRepository;
            _emailService = emailService;
        }

        public async Task<IError> InviteEmployee(bool IsManager,int UserId,string Host, String email, bool AsManager)
        {

            if (!IsManager)
                return new ManagerAuthorizationError();

            Employees employee = await _employeesRepository.GetByEmail(email);

            // check if Employee already exist
            if (employee != null)
                return new EmailAlreadyExistsError();

            Invites invite = await _invitesRepository.GetByEmail(email);

            // check if the requested email already invited
            if (invite != null)
                return new AlreadyInvitedError();


            Employees inviter = await _employeesRepository.GetEmployee(UserId);

            string SecretKey = RandomString.GetString(Types.ALPHABET_LOWERCASE, 10);

            // send SecretKey to email with view 
            _emailService.SendInviteEmail(inviter, email, SecretKey, Host);


            await _invitesRepository.CreateInvite(new Invites { inviteeEmail = email, AsManager = AsManager, inviter = inviter, SecretKey = SecretKey.Hash() });
            return null;
        }

        public async Task<Invites> GetInvite(string email, string SecretKey)
        {
            return  await _invitesRepository.GetByEmailAndSecretKey(email, SecretKey.Hash());
        }
    }
}
