using Core.Entities;

namespace Core.Abstract
{
    public interface IApplicationFormRepository: IRepository<ApplicationForm>
    {
        public Task<IEnumerable<ApplicationForm>> GetAllItemsAsync();
    }
}
