using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHouse.Awards.Infrastructure.Dtos.Common
{
    public record FileDto(byte[] fileBytes, string address)
    {
    }
}
