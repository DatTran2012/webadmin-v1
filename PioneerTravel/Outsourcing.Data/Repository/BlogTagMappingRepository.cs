﻿using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Repository
{
    public class BlogTagMappingRepository : RepositoryBase<BlogTagMapping>, IBlogTagMappingRepository
    {
        public BlogTagMappingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IBlogTagMappingRepository : IRepository<BlogTagMapping>
    {

    }
}
