
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Inspinia_MVC5_SeedProject.Models
{

using System;
    using System.Collections.Generic;
    
public partial class JobAd
{

    public int adId { get; set; }

    public Nullable<int> seats { get; set; }

    public string qualification { get; set; }

    public string exprience { get; set; }

    public string careerLevel { get; set; }

    public Nullable<System.DateTime> lastDateToApply { get; set; }

    public string salaryType { get; set; }

    public string category1 { get; set; }

    public Nullable<decimal> maxPrice { get; set; }



    public virtual Ad Ad { get; set; }

}

}
