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
    public class OTPRequestManager : Repository<OTPRequest, ApplicationDbContext>
    {
        public DbSet<OTPRequest> dbSet;
        public ApplicationDbContext context;
        public OTPRequestManager(ApplicationDbContext _context) : base(_context)
        {
            dbSet = _context.Set<OTPRequest>();
            context = _context;
        }



        // public async Task<bool> CreateOTPRequest(OTPRequest request)
        // {
        //     var res = context.OTPRequests.Add(request);
        //     await context.SaveChangesAsync();
        //     return true;
        // }

    }

}