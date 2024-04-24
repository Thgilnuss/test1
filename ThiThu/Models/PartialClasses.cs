using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThiThu.Models
{
	[MetadataType(typeof(BlogSetMetadata))]
	public partial class BlogSet
	{
	}
	[MetadataType(typeof(PostSetMetadata))]
	public partial class PostSet
	{

	}
}