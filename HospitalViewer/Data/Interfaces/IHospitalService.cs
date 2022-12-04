using HospitalViewer.Data.DTOs;
using HospitalViewer.Data.Models;

namespace HospitalViewer.Data.Interfaces
{
    public interface IHospitalService
    {
        Task<Contact> AddEditContact(Contact contact);
        Task<Hospital> AddEditHospital(Hospital hospital);
        Task<IQueryable<ContactDisplayModel>> GetHospitalContacts(long hospitalId, bool includePrivateContacts = false);
        Task<IQueryable<HospitalDisplayModel>> GetHospitals(string? Zip = null);
        Task LinkContact(HospitalContact contact);
        Task RemoveContact(long contactId);
        Task RemoveContactLink(long hospitalId, long contactId);
        Task RemoveHospital(long hospitalId);
    }
}