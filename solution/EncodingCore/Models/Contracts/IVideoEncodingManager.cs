using System;
using System.Threading.Tasks;

namespace EncodingCore.Models.Contracts
{
    public interface IVideoEncodingManager
    {
        Task Process();
    }
}
