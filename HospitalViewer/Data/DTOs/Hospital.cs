#nullable disable

using System.ComponentModel.DataAnnotations;

namespace HospitalViewer.Data.DTOs
{
    public class Hospital
    {
        [Key]
        public long HospitalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }

        public string AddressLine1 {  get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressZip { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public DateTime? DeleteDate { get; set; }

        public virtual IQueryable<HospitalContact> HospitalContacts { get; set; } 
    }
}
