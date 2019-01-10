using System.Threading.Tasks;

namespace Player.Interfaces
{
    public interface IPathService
    {
        string InternalFolder { get; }
        Task<string> OpenFolder();
    }
}
