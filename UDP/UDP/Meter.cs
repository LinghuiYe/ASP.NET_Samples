//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UDP
{
    using System;
    using System.Collections.Generic;
    
    public partial class Meter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Meter()
        {
            this.Wells = new HashSet<Well>();
        }
    
        public int Id { get; set; }
        public string Hostname { get; set; }
        public string MeterName { get; set; }
        public string SerialNumber { get; set; }
        public string Tag { get; set; }
        public string IP_Address { get; set; }
        public Nullable<int> Port { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Well> Wells { get; set; }
    }
}
