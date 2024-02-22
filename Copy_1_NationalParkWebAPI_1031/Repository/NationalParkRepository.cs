using Copy_1_NationalParkWebAPI_1031.Models;
using Copy_1_NationalParkWebAPI_1031.Repository;
using Copy_1_NationalParkWebAPI_1031.Repository.IRepository;

namespace Copy_1_NationalParkWebAPI_1031.Repository
{
    public class NationalParkRepository : Repository<NationalPark>,INationalParkRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public NationalParkRepository(IHttpClientFactory httpClientFactory ): base( httpClientFactory )
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
