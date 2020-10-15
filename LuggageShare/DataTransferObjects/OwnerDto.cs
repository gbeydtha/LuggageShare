﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuggageShare.DataTransferObjects
{
    public class OwnerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

       public IEnumerable<AccountDto> Accounts { get; set; }


    }
}