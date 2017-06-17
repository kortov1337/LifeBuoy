using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lifebuoy.Models;
using System.Data.Entity;

namespace Lifebuoy.DAL
{
    public class OffersContext:DbContext
    {
        public DbSet<Offers> Offers { get; set; }
        public DbSet<ModeratedOffers> ModeratedOffers { get; set; }
        public DbSet<ProvidersOffers> ProviderOffers { get; set; }
        public DbSet<ProvidersRequests> ProvidersRequests { get; set; }
        public DbSet<UsersVotes> UsersVotes { get; set; }
        public DbSet<Reviews> UsersReviews { get; set; }
        public DbSet<SkinDetails>SkinsDetails { get; set; }
    }
}