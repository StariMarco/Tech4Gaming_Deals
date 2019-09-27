using System;
using System.IO;
using System.Threading.Tasks;

namespace Tech4Gaming_Deals.Interfaces
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
