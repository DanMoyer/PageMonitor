using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PageMonitorRepository.Identity_Pages
{
	public  class AspNetUser
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public AspNetUser()
		{
			// ReSharper disable once VirtualMemberCallInContructor
			AspNetUserClaims = new HashSet<AspNetUserClaim>();
			// ReSharper disable once VirtualMemberCallInContructor
			AspNetUserLogins = new HashSet<AspNetUserLogin>();
			// ReSharper disable once VirtualMemberCallInContructor
			AspNetRoles = new HashSet<AspNetRole>();
		}

		public string Id { get; set; }

		[StringLength(256)]
		public string Email { get; set; }

		public bool EmailConfirmed { get; set; }

		public string PasswordHash { get; set; }

		public string SecurityStamp { get; set; }

		public string PhoneNumber { get; set; }

		public bool PhoneNumberConfirmed { get; set; }

		public bool TwoFactorEnabled { get; set; }

		public DateTime? LockoutEndDateUtc { get; set; }

		public bool LockoutEnabled { get; set; }

		public int AccessFailedCount { get; set; }

		[Required]
		[StringLength(256)]
		public string UserName { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
	}
}
