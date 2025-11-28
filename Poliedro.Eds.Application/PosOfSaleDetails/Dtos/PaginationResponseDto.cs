using System;
using System.Collections.Generic;
using System.Text;

namespace Poliedro.Eds.Application.PosOfSaleDetails.Dtos
{
    public class PaginationResponseDto<T>
    {
        public List<T> Data { get; set; }

        public int TotalPages { get; set; }

        public int TotalRows { get; set; }
    }
}
