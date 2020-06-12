﻿using Cat_Mouse.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat_Mouse.DbContextModel
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<RegistrationOrder> RegistrationOrders { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
