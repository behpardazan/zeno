﻿using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopDocument : BaseRepository<ShopDocument>
    {
        private DatabaseEntities _context;
        public Entity_ShopDocument(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
    }
}
