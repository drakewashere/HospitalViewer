using Duende.IdentityServer.Services;
using HospitalViewer.Data.DTOs;
using HospitalViewer.Data.Interfaces;

namespace HospitalViewer.EndpointHandlers
{
    public class ContactLinkEndpoints
    {
        public static async Task<IResult> LinkContact(HospitalContact contact, IHospitalService hospitalService)
        {
            try
            {
                await hospitalService.LinkContact(contact);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> UnlinkContact(long hospitalId, long contactId, IHospitalService hospitalService)
        {
            try
            {
                await hospitalService.RemoveContactLink(hospitalId, contactId);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
