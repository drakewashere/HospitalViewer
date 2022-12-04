#nullable disable

namespace HospitalViewer.Data.Models
{
    public class HospitalDisplayModel
    {
        public long HospitalId { get; set; }
        public List<ContactDisplayModel> Contacts { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressZip { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }

        private IEnumerable<string> _addressDisplay { get; set; }
        private IEnumerable<string> AddressDisplayLines
            => _addressDisplay ??= (new List<string>()
            {
                AddressLine1,
                AddressLine2,
                AddressLine3,
                $"{AddressCity}, {AddressState} {AddressZip}"
            }).Where(l => !string.IsNullOrEmpty(l));

        public string AddressDisplayHtml
            => string.Join("<br/>", AddressDisplayLines);
    }
}
