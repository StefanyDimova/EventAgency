using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAgency.Data.Common
{
    public static class ExceptionMessages
    {
        public const string SoftDeleteOnNonSoftDeletableEntity = "Soft Delete can not be performed on an Entity which does not support Soft Delete";
    }
}
