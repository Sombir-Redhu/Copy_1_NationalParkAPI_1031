using Copy_1_NationalParkAPI_1031.Models;

namespace Copy_1_NationalParkAPI_1031.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();
        Trail GetTrail (int trailId);   
        ICollection<Trail> GetTrailsInNationalPark(int nationalParkId);
        bool TrailExists(int trailId);
        bool TrailExists(string trailName);
        bool CreateTrail(Trail trail);
        bool UpdateTrail(Trail trail);
        bool DeleteTrail(Trail trail);
        bool Save();
    }
}
