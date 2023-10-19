using Microsoft.EntityFrameworkCore;
using Stack.DAL;
using Stack.Entities.DatabaseEntities.Auth;
using Stack.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack.Core.Managers.Modules.Auth
{
    public class RegistrationRequestManager : Repository<RegistrationRequest, ApplicationDbContext>
    {
        public DbSet<RegistrationRequest> dbSet;
        public ApplicationDbContext context;
        public RegistrationRequestManager(ApplicationDbContext _context) : base(_context)
        {
            dbSet = _context.Set<RegistrationRequest>();
            context = _context;
        }


        public async Task<RegistrationRequest> GetRegistrationRequestByPhoneNumber(string phoneNumber)
        {

            return await Task.Run(() =>
            {
                return context.RegistrationRequests.Where(t => t.PhoneNumber == phoneNumber).FirstOrDefault();
            });
        }



        public async Task<bool> CreateRegistrationRequest(RegistrationRequest request)
        {

            var res = context.RegistrationRequests.Add(request);
            await context.SaveChangesAsync();
            return true;
        }

    }

}