using System;
using System.ComponentModel.DataAnnotations;

namespace dashboard.Models
{
    internal class DatatypeAttribute : Attribute
    {
        public DatatypeAttribute(DataType password)
        {
        }
    }
}