using HospitalViewer.Data.DTOs;
using HospitalViewer.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalViewer.EndpointHandlers
{
    public static class ContactEndpoints
    {
        public static async Task<IResult> GetContactsForHospital(long hospitalId, IHospitalService hospitalService)
        {
            try
            {
                var hospitals = await hospitalService.GetHospitalContacts(hospitalId);

                if (hospitals.Any())
                    return Results.Ok(hospitals);
                return Results.NotFound();
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> AddEditContact(Contact editContact, IHospitalService hospitalService)
        {
            try
            {
                var contact = await hospitalService.AddEditContact(editContact);
                var add = editContact.ContactId == 0;
                if (contact.ContactId != 0)
                    return Results.Ok();

                return Results.Problem($"Unable to {(add ? "add" : "edit")} contact");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> RemoveContact(long contactId, IHospitalService hospitalService)
        {
            try
            {
                await hospitalService.RemoveContact(contactId);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
