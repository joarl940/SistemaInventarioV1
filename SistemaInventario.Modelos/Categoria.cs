using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Nombre es requerido")]
        [MaxLength(60,ErrorMessage ="Nombre debe ser de Maximo 60 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripcion es requerido")]
        [MaxLength(100, ErrorMessage = "Descripcion debe ser de Maximo 100 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage ="Esatdo es requerido")]
        public bool Estado { get; set; }
    }
}
