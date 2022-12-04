#nullable disable

using System.ComponentModel.DataAnnotations;

namespace HospitalViewer.Data.DTOs
{
    public class Contact
    {
        [Key]
        public long ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumberOverride { get; set; }
        public string EmailOverride { get; set; }
        public DateTime? DeleteDate { get; set; }
        public virtual IQueryable<HospitalContact> HospitalContacts { get; set; }
    }
}
