using Domain.Base;

namespace Application.Services.Interfaces
{
    public class BaseService
    {
        protected internal IUnitOfWork UnitOfWork { get; set; }
        public BaseService(IUnitOfWork unitOfWork) {
        this.UnitOfWork = unitOfWork;
        }
    }
}
