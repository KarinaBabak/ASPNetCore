using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindSystem.Data.DTOModels
{
    public class CategoryDto : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    }
}
