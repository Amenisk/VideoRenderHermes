//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VideoRenderHermes.ADOApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int ClientId { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual Tag Tag { get; set; }
    }
}