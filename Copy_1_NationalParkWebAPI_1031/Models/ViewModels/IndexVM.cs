namespace Copy_1_NationalParkWebAPI_1031.Models.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<NationalPark> NationalParksList { get; set; }
        public IEnumerable<Trail> TrailList { get; set; }
    }
}
