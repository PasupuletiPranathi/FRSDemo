using System;
using System.Collections.Generic;

namespace FilmRentalStoreProjectDAL.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Usertable> Usertables { get; set; } = new List<Usertable>();
}
