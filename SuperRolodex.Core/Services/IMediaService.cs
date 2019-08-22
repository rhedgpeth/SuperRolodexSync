using System.Threading.Tasks;

namespace SuperRolodex.Core.Services
{
    public interface IMediaService
    {
        Task<byte[]> PickPhotoAsync();
    }
}
