using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageMonitor.Repository
{
	public class PageMonitorRepository<T> where T : class
	{
		private readonly PageMonitor _context = new PageMonitor();
		public DbSet<T> DbSet { get; set; }

		public PageMonitorRepository()
		{
			DbSet = _context.Set<T>();
		}

		public List<T> GetAll()
		{
			return DbSet.ToList();
		}
		public void Add(T entity)
		{
			DbSet.Add(entity);
		}

		public void SaveChanges()
		{
			try
			{
				_context.SaveChanges();
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
	}
}
