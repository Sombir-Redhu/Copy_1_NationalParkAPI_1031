using Microsoft.AspNetCore.Mvc.Rendering;

namespace Copy_1_NationalParkWebAPI_1031.Models.ViewModels
{
    public class TrailVM
    {
        public Trail Trail { get; set; }
        public IEnumerable<SelectListItem> nationalParkList { get; set; }   

    }
}
