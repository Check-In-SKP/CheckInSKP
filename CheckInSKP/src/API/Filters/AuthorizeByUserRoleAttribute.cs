﻿using Microsoft.AspNetCore.Mvc;

namespace API.Filters
{
    public class AuthorizeByUserRoleAttribute : TypeFilterAttribute
    {
        public AuthorizeByUserRoleAttribute(params int[] roleIds) : base(typeof(AuthorizeByUserRoleFilter))
        {
            Arguments = new object[] { roleIds };
        }
    }
}
