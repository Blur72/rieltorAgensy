//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace rieltorAgensy
{
    using System;
    using System.Collections.Generic;
    
    public partial class Deals
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Deals()
        {
            this.Payments = new HashSet<Payments>();
        }
    
        public int DealID { get; set; }
        public Nullable<int> PropertyID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<int> RealtorID { get; set; }
        public Nullable<System.DateTime> DealDate { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Status { get; set; }
        public Nullable<int> DealStatusID { get; set; }
    
        public virtual Clients Clients { get; set; }
        public virtual Propertiezzz Propertiezzz { get; set; }
        public virtual Realtors Realtors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payments> Payments { get; set; }
        public virtual DealStatuses DealStatuses { get; set; }
    }
}