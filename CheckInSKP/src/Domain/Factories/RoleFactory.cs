﻿using CheckInSKP.Domain.Entities;

namespace CheckInSKP.Domain.Factories
{
    public class RoleFactory
    {
        public Role CreateRole(int id, string name)
        {
            return new Role(id, name);
        }

        public Role CreateNewRole(string name)
        {
            return new Role(name);
        }
    }
}
