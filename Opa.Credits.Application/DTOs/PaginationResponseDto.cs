using System.Collections.Generic;

namespace Opa.Credits.Application.DTOs;

public class PaginationResponseDto<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
