using HospitalViewer.Data;
using HospitalViewer.Data.DTOs;
using HospitalViewer.Data.Interfaces;

namespace HospitalViewer.EndpointHandlers
{
    public static class TestDataEndpoints
    {
        public static async Task<IResult> Generate(IHospitalService service)
        {
            var hospitals = new List<Hospital>
            {
                new Hospital
                {
                    Name = "Test",
                    Description = "Test",
                    PhoneNumber= "(480) 555-5555",
                    AddressLine1 = "123 Test St",
                    AddressCity = "Phoenix",
                    AddressState = "AZ",
                    AddressZip = "88888"
                },
                new Hospital
                {
                    Name = "Test2",
                    PhoneNumber= "(480) 555-5555",
                    AddressLine1 = "123 Test St",
                    AddressCity = "Phoenix",
                    AddressState = "AZ",
                    AddressZip = "88888"
                },
                new Hospital
                {
                    Name = "Test3",
                    Description = "Test3",
                    AddressLine1 = "123 Test St",
                    AddressCity = "Phoenix",
                    AddressState = "AZ",
                    AddressZip = "88888"
                }
            };

            hospitals.ForEach(h => h = service.AddEditHospital(h).Result);

            return Results.Ok("Added entities");
        }

        public static async Task<IResult> Truncate(ApplicationDbContext context)
        {
            context.Hospitals.RemoveRange(context.Hospitals);
            await context.SaveChangesAsync();
            return Results.Ok("Removed entities");
        }
    }
}
