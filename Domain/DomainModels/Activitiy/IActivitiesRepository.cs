using Domain.Activitiy;

namespace Domain.DomainModels.Activitiy
{
    public interface IActivitiesRepository
    {
        public void AddChangeStatusActivities(List<Activities> activites) { throw new NotImplementedException(); }
        public Task<IEnumerable<Activities>> GetAllActivities(int pageIndex = 1, int pageSize = 30) { throw new NotImplementedException(); }
        public Task<int> count();
    }
}
