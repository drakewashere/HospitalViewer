#nullable disable


using System.ComponentModel.DataAnnotations;

namespace HospitalViewer.Data.DTOs
{
    public enum HospitalContactRoleId
    {
        Provider = 1,
        Scheduling = 2,
        Billing = 3
    }
    public class HospitalContactRole
    {
        [Key]
        public HospitalContactRoleId HospitalContactRoleId { get; set;}
        public string HospitalContactRoleName { get; set; }
    }
}
