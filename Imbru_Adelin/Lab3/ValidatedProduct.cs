﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public record ValidatedProduct(ProductCode ProductCode, Quantity Quantity, Price Price, Address Address);
};

