using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PageMonitorRepository.Identity_Pages
{
	public class AspNetRole
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public AspNetRole()
		{
			// ReSharper disable once VirtualMemberCallInContructor
			AspNetUsers = new HashSet<AspNetUser>();
		}

		public string Id { get; set; }

		[Required]
		[StringLength(256)]
		public string Name { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
	}
}
