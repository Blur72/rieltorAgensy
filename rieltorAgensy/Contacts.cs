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
    
    public partial class Contacts
    {
        public int ContactID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<int> RealtorID { get; set; }
        public string ContactType { get; set; }
        public string ContactValue { get; set; }
    
        public virtual Clients Clients { get; set; }
        public virtual Realtors Realtors { get; set; }
    }
}
