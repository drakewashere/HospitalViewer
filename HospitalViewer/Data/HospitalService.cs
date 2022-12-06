using HospitalViewer.Data.DTOs;
using HospitalViewer.Data.Interfaces;
using HospitalViewer.Data.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Linq;
using System.Linq.Expressions;

namespace HospitalViewer.Data
{
    public class HospitalService : IHospitalService
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Hospital> Hospitals { get { return _context.Hospitals; } }
        private DbSet<Contact> Contacts { get { return _context.Contacts; } }
        public HospitalService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static Expression<Func<Hospital, HospitalDisplayModel>> HospitalDisplay
            => h => new HospitalDisplayModel()
            {
                Name = h.Name,
                Description = h.Description,
                PhoneNumber = h.PhoneNumber,
                AddressLine1 = h.AddressLine1,
                AddressLine2 = h.AddressLine2,
                AddressLine3 = h.AddressLine3,
                AddressZip = h.AddressZip,
                AddressCity = h.AddressCity,
                AddressState = h.AddressState,
                Contacts = h.HospitalContacts.AsQueryable().Where(hc => hc.Contact.DeleteDate == null).Select(ContactsDisplay).ToList()
            };

        private static Expression<Func<HospitalContact, ContactDisplayModel>> ContactsDisplay
            => hc => new ContactDisplayModel()
            {
                HospitalContactRole = hc.HospitalContactRoleId,
                FirstName = hc.Contact.FirstName,
                LastName = hc.Contact.LastName,
                PhoneNumber = !string.IsNullOrEmpty(hc.Contact.PhoneNumberOverride)
                                    ? hc.Contact.PhoneNumberOverride
                                    : $"{hc.Hospital.PhoneNumber} x{hc.HospitalPhoneExtension}",
                Email = !string.IsNullOrEmpty(hc.Contact.EmailOverride)
                                    ? hc.Contact.EmailOverride
                                    : hc.HostpialEmail,
            };

        public async Task<IQueryable<HospitalDisplayModel>> GetHospitals(string? Zip = null)
             => await Task.Run(() => Hospitals
                    .Where(h =>
                        h.DeleteDate == null
                        & (string.IsNullOrEmpty(Zip) || h.AddressZip.Equals(Zip))
                    )
                    .Select(HospitalDisplay));

        public async Task<IQueryable<ContactDisplayModel>> GetHospitalContacts(long hospitalId, bool includePrivateContacts = false)
            => await Task.Run(() => Hospitals
                    .Where(h => h.DeleteDate == null && h.HospitalId == hospitalId)
                    .SelectMany(h => h.HospitalContacts)
                    .Where(hc => hc.Contact.DeleteDate == null
                        && (hc.DisplayInDirectory || includePrivateContacts)
                    )
                    .Select(ContactsDisplay));

        public async Task<Hospital> AddEditHospital(Hospital hospital)
        {
            if (hospital.HospitalId == 0)
            {
                await _context.AddAsync(hospital);
            }
            else
            {
                _context.Update(hospital);
            }
            await _context.SaveChangesAsync();
            return hospital;
        }

        public async Task RemoveHospital(long hospitalId)
        {
            var hospital = await Hospitals.FirstOrDefaultAsync(h => h.HospitalId == hospitalId);
            if (hospital != null)
            {
                hospital.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Contact> AddEditContact(Contact contact)
        {
            if (contact.ContactId == 0)
            {
                await _context.AddAsync(contact);
            }
            else
            {
                _context.Update(contact);
            }
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task RemoveContact(long contactId)
        {
            var contact = await Contacts.FirstOrDefaultAsync(c => c.ContactId == contactId);
            if (contact != null)
            {
                contact.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

        }

        public async Task LinkContact(HospitalContact contact)
        {
            if (contact.HospitalId == 0 || contact.ContactId == 0)
                return;
            if (await _context.HospitalContacts.AnyAsync(hc => hc.Hospital == contact.Hospital && hc.ContactId == contact.ContactId))
                return;
            await _context.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveContactLink(long hospitalId, long contactId)
        {
            var match = _context.HospitalContacts.Where(hc => hc.HospitalId == hospitalId && hc.ContactId == contactId);
            _context.RemoveRange(match);
            await _context.SaveChangesAsync();
        }
    }
}
