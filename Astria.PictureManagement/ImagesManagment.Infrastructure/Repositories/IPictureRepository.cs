using ImagesManagment.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesManagment.Infrastructure.Repositories
{
	public interface IPictureRepository<T> where T : class
	{
		Task<List<T>> GetAll();
		Task<T> GetById(Guid id);
		Task<T> Create(T image);
		Task Update(T image);
		Task Delete(Guid id);
	}
}
