using TestGrpc;

namespace GrpcClient
{
    // for simplifying dependency injection
    public interface IGrpcClientService
    {
        Task<GetResponse> Get(string id);
    }
}
