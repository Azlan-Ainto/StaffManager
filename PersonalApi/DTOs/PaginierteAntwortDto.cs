using System.Collections.Generic;

namespace PersonalApi.DTOs;

public class PaginierteAntwortDto<T>
{
    public IEnumerable<T> Elemente { get; set; } = new List<T>();


}
