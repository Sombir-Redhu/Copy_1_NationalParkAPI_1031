using Copy_1_NationalParkAPI_1031.Models;

namespace Copy_1_NationalParkAPI_1031.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();
        NationalPark GetNationalPark(int nationalParkId);
        bool NationalParkExists(int nationalParkId);
        bool NationalParkExists(string nationalParkName);
        bool CreateNationPark(NationalPark nationalPark);
        bool UpdateNationPark(NationalPark nationalPark);
        bool DeleteNationPark(NationalPark nationalPark);
        bool Save();
    }
}
