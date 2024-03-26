using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EdriveAuto.Common;

namespace EdriveAuto.GenericRepository;

public interface IBaseRepositoryModel : IBaseModel
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	int IBaseModel.ID
	{
		get => ID;
		set => ID = value;
	}
}