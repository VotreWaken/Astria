using Astria.QueryRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Application.BoundedContexts.UserImageManagment.QueryObjects
{
    public class ImageInfo : IQueryEntity
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
    }
}
