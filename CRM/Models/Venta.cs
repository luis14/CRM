//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Venta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Venta()
        {
            this.Ventas_x_Productos = new HashSet<Ventas_x_Productos>();
        }
    
        public int venta_id { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public Nullable<int> descuento { get; set; }
        public Nullable<int> comision { get; set; }
        public Nullable<int> fvendedor_id { get; set; }
        public Nullable<int> cliente_id { get; set; }
        public string productolista { get; set; }
        public string errorMsj { get; set; }
        public virtual Vendedore Vendedore { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ventas_x_Productos> Ventas_x_Productos { get; set; }
    }
}
