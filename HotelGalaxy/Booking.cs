//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelGalaxy
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        public int ID_Booking { get; set; }
        public Nullable<int> Гость { get; set; }
        public Nullable<int> Номер { get; set; }
        public string Количество_человек { get; set; }
        public Nullable<System.DateTime> Дата_заезда { get; set; }
        public Nullable<System.DateTime> Дата_выезда { get; set; }
        public string Количество_дней_проживания { get; set; }
    
        public virtual Guest Guest { get; set; }
        public virtual Room Room { get; set; }
    }
}
