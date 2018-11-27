
using System.Threading.Tasks;

namespace stPetes.Services
{
    //interface for any pdf related services 
    public interface Ipdf
    {
        Task<int> OpenPdf(string sUrl);
    }
}
