
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
    
public partial class Chat
{

    public int Id { get; set; }

    public string sentFrom { get; set; }

    public string sentTo { get; set; }

    public string message { get; set; }

    public System.DateTime time { get; set; }

    public Nullable<System.DateTime> SeenAt { get; set; }



    public virtual AspNetUser AspNetUser { get; set; }

    public virtual AspNetUser AspNetUser1 { get; set; }

}

}