using System.Threading.Tasks;

namespace KidsList.Services.Kids
{
    public interface IKidService
    {
        Task<KidDto> AddKidToFamily(int userId, AddKidRequest addKidRequest);
    }
}
