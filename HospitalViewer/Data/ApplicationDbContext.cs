#nullable disable

using Duende.IdentityServer.EntityFramework.Options;
using HospitalViewer.Data.DTOs;
using HospitalViewer.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

namespace HospitalViewer.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<HospitalContactRole> HospitalContactRoles { get; set; }
        public DbSet<HospitalContact> HospitalContacts { get; set; }
    }
}