using System;
using System.Collections.Generic;

namespace FilmRentalStoreProjectWebAPI.Models;

public partial class FilmText
{
    public int FilmId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}
