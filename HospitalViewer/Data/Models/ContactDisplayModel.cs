#nullable disable
using HospitalViewer.Data.DTOs;

namespace HospitalViewer.Data.Models
{
    public class ContactDisplayModel
    {
        public HospitalContactRoleId HospitalContactRole { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
