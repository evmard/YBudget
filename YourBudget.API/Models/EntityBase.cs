﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourBudget.API.Models
{
    public abstract class EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
    }
}