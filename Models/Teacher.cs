using System;
using System.Collections.Generic;

namespace LAB_3_SQL_ORM.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string? SubjectField { get; set; }

    public int? FkPersonId { get; set; }

    public virtual Person? FkPerson { get; set; }
}
