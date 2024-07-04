using Rent_A_Car.Models;

namespace Rent_A_Car.ViewModels
{
    public class RequestsIndexViewModel
    {
        public List<Requests> AllRequests { get; set; }
        public List<Requests> ApprovedRequests { get;set; }
        public List<Requests> UnpprovedRequests { get; set; }
    }
}
