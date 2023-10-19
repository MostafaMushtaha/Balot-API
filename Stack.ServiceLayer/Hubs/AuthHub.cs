using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Stack.Core;
using Stack.DTOs;
using Stack.DTOs.Models;
using Stack.DTOs.Requests;
using Stack.Entities.Models.Modules.Common;
using Stack.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stack.API.Hubs
{
    [Authorize]
    public class AuthHub : Hub
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        protected IHubContext<AuthHub> _context;

        public AuthHub(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<AuthHub> context)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this._context = context;
        }

        public async override Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;
            var user = await unitOfWork.UserManager.FindByNameAsync(username);
            if (user == null) return;

            // Wrap in a transaction for atomicity
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var userConnectionIDQ = await unitOfWork.UserConnectionIDsManager.GetAsync(t => t.UserID == user.Id);
                var userConnectionID = userConnectionIDQ.FirstOrDefault();

                if (userConnectionID is not null)
                {
                    // Remove any existing connections directly
                    await unitOfWork.UserConnectionIDsManager.RemoveAsync(userConnectionID);
                }

                var conId = new UserConnectionID
                {
                    ID = Context.ConnectionId,
                    UserID = user.Id
                };
                await unitOfWork.UserConnectionIDsManager.CreateAsync(conId);

                await unitOfWork.SaveChangesAsync();
                await unitOfWork.CommitTransactionAsync();

            }
            catch
            {
                await unitOfWork.RollbackTransactionAsync();
                throw;  // or handle the exception
            }
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User.Identity.Name;
            var user = await unitOfWork.UserManager.FindByNameAsync(username);
            if (user != null)
            {
                await unitOfWork.BeginTransactionAsync();

                try
                {
                    var conIdQuery = await unitOfWork.UserConnectionIDsManager.GetAsync(t => t.UserID == user.Id); //User Connection exists
                    var connection = conIdQuery.FirstOrDefault();

                    if (connection is not null)
                    {
                        // Remove any existing connections directly
                        await unitOfWork.UserConnectionIDsManager.RemoveAsync(connection);

                        user.LastLogin = DateTime.UtcNow;
                        await unitOfWork.UserManager.UpdateAsync(user);

                        await unitOfWork.SaveChangesAsync();
                        await unitOfWork.CommitTransactionAsync();
                    }

                }
                catch
                {
                    await unitOfWork.RollbackTransactionAsync();
                    throw;  // or handle the exception
                }
            }
        }
    }
}
