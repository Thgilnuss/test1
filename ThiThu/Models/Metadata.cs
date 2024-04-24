using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThiThu.Models
{
	public class BlogSetMetadata
	{
		[Required(ErrorMessage ="Tên bài viết là bắt buộc")]
		[StringLength(20, MinimumLength =5,ErrorMessage ="Tên bài viết khoảng từ 5 đến 20")]
		[Display(Name="Tên bài viết")]
		public string Name ;

		[Required(ErrorMessage ="Đường dẫn là bắt buộc")]
		[StringLength(100)]
		[Display(Name ="Đường dẫn")]
		public string Url ;

		[Required(ErrorMessage ="Mô tả là bắt buộc")]
		[StringLength(100)]
		[Display(Name ="Mô tả")]
		public string Description ;

		[Required(ErrorMessage ="Tác giả là bắt buộc")]
		[StringLength(100)]
		[Display(Name ="Tác giả")]
		public string Owner ;

		[Required(ErrorMessage ="Cấp độ là bắt buộc")]
		[Range(1,100,ErrorMessage ="Cấp đọ phải nằm trong khoảng từ 1 đến 100")]
		[Display(Name ="Cấp độ")]
		public Nullable<int> Rank ;
	}

	public class PostSetMetadata
	{
		[Required(ErrorMessage ="Tiêu đề là bắt buộc")]
		[StringLength(50, MinimumLength = 5, ErrorMessage ="Tiêu đề phải từ 5 đến 50")]
		[Display(Name ="Tiêu đề")]
		public string Title ;

		[Required(ErrorMessage ="Nội dung là bắt buộc")]
		[StringLength(100)]
		[Display(Name ="Nội dung")]
		public string Content ;

		[Required(ErrorMessage ="Id là bắt buộc")]
		[Range(1, int.MaxValue, ErrorMessage ="Blog Id khac 0 ")]
		[Display(Name ="BlogIdPost")]	
		public int BlogBlogId ;

		[Required(ErrorMessage = "Ngày là bắt buộc")]
		[DataType(DataType.Date, ErrorMessage ="Nhập đúng định dạng dd/MM/yyy")]
		[DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}", ApplyFormatInEditMode =true)]
		[Display(Name = "Ngày tạo")]
		public Nullable<System.DateTime> CreatedDate;
	}
}