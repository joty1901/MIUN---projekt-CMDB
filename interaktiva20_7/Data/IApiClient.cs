using System.Threading.Tasks;

namespace interaktiva20_7.Data
{
    public interface IApiClient{
            Task<T> GetASync<T>(string endpoint);
    }
   
}
