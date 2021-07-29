﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Remotely.Server.Services;
using System;
using System.Text;

namespace Remotely.Server.Auth
{
    public class ApiAuthorizationFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public ApiAuthorizationFilter(IDataService dataService)
        {
            DataService = dataService;
        }

        private IDataService DataService { get; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var orgID = DataService.GetUserByNameWithOrg(context.HttpContext.User.Identity.Name)?.OrganizationID;
                context.HttpContext.Request.Headers["OrganizationID"] = orgID;
                return;
            }

            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var result))
            {
                var tokenType = result.ToString().Split(" ")[0].Trim();
                
                switch(tokenType)
                {
                    case "Basic":
                        var encodedToken = result.ToString().Split(" ")[1].Trim();

                        byte[] data = Convert.FromBase64String(encodedToken);
                        string decodedString = Encoding.UTF8.GetString(data);
                        var keyId = decodedString.ToString().Split(":")[0]?.Trim();
                        var apiSecret = decodedString.ToString().Split(":")[1]?.Trim();
                        if (DataService.ValidateApiKey(keyId, apiSecret, context.HttpContext.Request.Path, context.HttpContext.Connection.RemoteIpAddress.ToString()))
                        {
                            var orgID = DataService.GetApiKey(keyId)?.OrganizationID;
                            context.HttpContext.Request.Headers["OrganizationID"] = orgID;
                            return;
                        }
                        break;
                }
                
            }

            context.Result = new UnauthorizedResult();
        }
    }
}
