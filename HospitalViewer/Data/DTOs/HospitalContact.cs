#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalViewer.Data.DTOs
{
    public class HospitalContact
    {
        [Key]
        public long HospitalContactId { get; set; }
        public long HospitalId { get; set; }
        public long ContactId { get; set; }
        public HospitalContactRoleId HospitalContactRoleId { get; set; }
        public string HospitalPhoneExtension { get; set; }
        public string HostpialEmail { get; set; }
        public bool DisplayInDirectory { get; set; }

        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }
        [ForeignKey("HospitalContactRoleId")]
        public virtual HospitalContactRole Role { get; set; }
    }
}
