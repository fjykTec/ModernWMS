
using System;
using System.ComponentModel.DataAnnotations;
namespace ModernWMS.Core.Models
{
    [Serializable]
    public abstract class BaseModel
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { get; set; } = 0;

    }
}
