
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
    
public partial class Bid
{

    public int Id { get; set; }

    public int adId { get; set; }

    public string postedBy { get; set; }

    public System.DateTime time { get; set; }

    public decimal price { get; set; }



    public virtual AspNetUser AspNetUser { get; set; }

    public virtual Ad Ad { get; set; }

}

}
