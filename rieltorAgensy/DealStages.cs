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
    
    public partial class DealStages
    {
        public int StageID { get; set; }
        public Nullable<int> DealID { get; set; }
        public string StageName { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual Deals Deals { get; set; }
    }
}