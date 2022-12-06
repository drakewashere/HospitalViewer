using HospitalViewer.Data.DTOs;
using HospitalViewer.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalViewer.EndpointHandlers
{
    public static class HospitalEndpoints
    {
        public static async Task<IResult> GetHospitals(string? zip, IHospitalService hospitalService)
        {
            try
            {
                var hospitals = await hospitalService.GetHospitals(zip);

                if (hospitals.Any())
                    return Results.Ok(hospitals);
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> AddEditHospital(Hospital editHospital, IHospitalService hospitalService)
        {
            try
            {
                var hospital = await hospitalService.AddEditHospital(editHospital);
                var add = editHospital.HospitalId == 0;
                if (hospital.HospitalId != 0)
                    return Results.Ok();

                return Results.Problem($"Unable to {(add ? "add" : "edit")} hospital");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> RemoveHospital(long hospitalId, IHospitalService hospitalService)
        {
            try
            {
                await hospitalService.RemoveHospital(hospitalId);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
