using System;
using System.Collections.Generic;

namespace LAB_3_SQL_ORM.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string? Class { get; set; }

    public int? FkPersonId { get; set; }

    public virtual Person? FkPerson { get; set; }
}
