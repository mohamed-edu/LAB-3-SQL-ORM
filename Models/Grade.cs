using System;
using System.Collections.Generic;

namespace LAB_3_SQL_ORM.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int? FkStudentId { get; set; }

    public int? FkCourseId { get; set; }

    public int? FkTeacherId { get; set; }

    public int? Grade1 { get; set; }

    public DateTime? GradeDate { get; set; }

    public virtual Course? FkCourse { get; set; }

    public virtual Student? FkStudent { get; set; }

    public virtual Teacher? FkTeacher { get; set; }
}
