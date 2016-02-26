using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace PageMonitorRepository.Monitor
{
	public class PageMonitorRepository<T> where T : class
	{
		//private readonly PageMonitorDb _context = new PageMonitorDb();

		protected PageMonitorDb Context { get; set; } = new PageMonitorDb();

		public DbSet<T> DbSet { get; set; }

		public PageMonitorRepository()
		{
			DbSet = Context.Set<T>();
		}

		public List<T> GetAll()
		{
			return DbSet.ToList();
		}

		public void Add(T entity)
		{
			DbSet.Add(entity);
		}

		public void Delete(T entity)
		{
			DbSet.Remove(entity);
		}

		public void SaveChanges()
		{
			try
			{
				Context.SaveChanges();
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						Trace.TraceInformation("Property: {0} Error: {1}",
							validationError.PropertyName,
							validationError.ErrorMessage);
					}
				}

			}
		}
		

		public void Dispose()
		{
			Context.Dispose();
		}
	}
}
