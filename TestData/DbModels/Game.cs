﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.DbModels
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public  decimal Price { get; set; }
        public string Description { get; set; }
        public float Rate { get; set; }
        public string ImageUrl { get; set; }

        public Guid? UserId { get; set; }
        public User User { get; set; }
    }
}
