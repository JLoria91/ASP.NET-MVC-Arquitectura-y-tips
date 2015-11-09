namespace Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public interface IAuditoria
    {
        int id { get; set; }
    }

    public enum AuditoriaTipo
    {
        Insertar = 1,
        Actualizar = 2,
        Eliminar = 3
    }

    [Table("Auditoria")]
    public partial class Auditoria
    {
        public long id { get; set; }
        public int Usuario { get; set; }
        public int Tabla_id { get; set; }
        public string Tabla { get; set; }
        public AuditoriaTipo Tipo { get; set; }
        public DateTime Fecha { get; set; }
    }
}
