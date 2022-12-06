using HospitalViewer.Data.DTOs;
using HospitalViewer.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        public static async Task<IResult> AddEditHospital(HttpRequest request, IHospitalService hospitalService)
        {
            try
            {
                var body = "";
                using (var sr = new StreamReader(request.Body))
                {
                    body = await sr.ReadToEndAsync();
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var editHospital = JsonSerializer.Deserialize<Hospital>(body, options);

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
