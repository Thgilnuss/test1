using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThiThu.Models;
using PagedList;
using System.Web.Services.Discovery;
using System.Reflection;
namespace ThiThu.Controllers
{
    public class PostSetsController : Controller
    {
        private DatabaseBloggingContextEntities db = new DatabaseBloggingContextEntities();
		
		// GET: PostSets
		public ActionResult Index(int? size, int? page, string sortProperty, string sortOrder)
		{
			// 1. Tạo biến ViewBag gồm sortOrder, searchValue, sortProperty và page
			if (sortOrder == "asc") ViewBag.sortOrder = "desc";
			if (sortOrder == "desc") ViewBag.sortOrder = "";
			if (sortOrder == "") ViewBag.sortOrder = "asc";
			ViewBag.sortProperty = sortProperty;
			ViewBag.page = page;

			// 2. Tạo danh sách chọn số trang
			List<SelectListItem> items = new List<SelectListItem>();
			items.Add(new SelectListItem { Text = "5", Value = "5" });
			items.Add(new SelectListItem { Text = "10", Value = "10" });
			items.Add(new SelectListItem { Text = "25", Value = "25" });

			// 2.1. Thiết lập số trang đang chọn vào danh sách List<SelectListItem> items
			foreach (var item in items)
			{
				if (item.Value == size.ToString()) item.Selected = true;
			}
			ViewBag.size = items;
			ViewBag.currentSize = size;


			var properties = typeof(PostSet).GetProperties();
			List<Tuple<string, bool, int>> list = new List<Tuple<string, bool, int>>();
			foreach (var item in properties)
			{
				int order = 999;
				var isVirtual = item.GetAccessors()[0].IsVirtual;

				if (item.Name == "Title") order = 1;
				Tuple<string, bool, int> t = new Tuple<string, bool, int>(item.Name, isVirtual, order);
				list.Add(t);
			}
			list = list.OrderBy(x => x.Item3).ToList();

			// 3.1. Tạo Heading sắp xếp cho các cột
			foreach (var item in list)
			{
				if (!item.Item2 && (item.Item1 == "Title"))
				{
					if (sortOrder == "desc" && sortProperty == item.Item1)
					{
						ViewBag.Headings += "<th><a href='?page=" + page + "&size=" + ViewBag.currentSize + "&sortProperty=" + item.Item1 + "&sortOrder=" +
							ViewBag.sortOrder  + "'>" + item.Item1 + "<i class='fa fa-fw fa-sort-desc'></i></th></a></th>";
					}
					else if (sortOrder == "asc" && sortProperty == item.Item1)
					{
						ViewBag.Headings += "<th><a href='?page=" + page + "&size=" + ViewBag.currentSize + "&sortProperty=" + item.Item1 + "&sortOrder=" +
							ViewBag.sortOrder + "'>" + item.Item1 + "<i class='fa fa-fw fa-sort-asc'></a></th>";
					}
					else
					{
						ViewBag.Headings += "<th><a href='?page=" + page + "&size=" + ViewBag.currentSize + "&sortProperty=" + item.Item1 + "&sortOrder=" +
						   ViewBag.sortOrder + "'>" + item.Item1 + "<i class='fa fa-fw fa-sort'></a></th>";
					}

				}
				else if (item.Item1 == "Title")
				{
					ViewBag.Headings += "<th>" + item.Item1 + "</th>";
				}
			}

			// 4. Truy vấn lấy tất cả đường dẫn
			var posts = from p in db.PostSets
						select p;

			// 5. Tạo thuộc tính sắp xếp mặc định là "LinkID"
			if (String.IsNullOrEmpty(sortProperty)) sortProperty = "Title";

			// 5. Sắp xếp tăng/giảm bằng phương thức OrderBy sử dụng trong thư viện Dynamic LINQ
			if (sortOrder == "desc") posts = posts.OrderBy(sortProperty + " desc");
			else if (sortOrder == "asc") posts = posts.OrderBy(sortProperty);
			else posts = posts.OrderBy("Title");

			

			// 5.2. Nếu page = null thì đặt lại là 1.
			page = page ?? 1; //if (page == null) page = 1;

			// 5.3. Tạo kích thước trang (pageSize), mặc định là 5.
			int pageSize = (size ?? 5);

			ViewBag.pageSize = pageSize;

			// 6. Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
			// nếu page = null thì lấy giá trị 1 cho biến pageNumber.
			int pageNumber = (page ?? 1);

			// 6.2 Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
			int checkTotal = (int)(posts.ToList().Count / pageSize) + 1;
			// Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
			if (pageNumber > checkTotal) pageNumber = checkTotal;

			// 7. Trả về các Link được phân trang theo kích thước và số trang.
			return View(posts.ToPagedList(pageNumber, pageSize));

		}

		// GET: PostSets/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostSet postSet = db.PostSets.Find(id);
            if (postSet == null)
            {
                return HttpNotFound();
            }
            return View(postSet);
        }

        // GET: PostSets/Create
        public ActionResult Create()
        {
            ViewBag.BlogBlogId = new SelectList(db.BlogSets, "BlogId", "Name");
            return View();
        }

        // POST: PostSets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,Title,Content,BlogBlogId,CreatedDate")] PostSet postSet)
        {
            if (ModelState.IsValid)
            {
                db.PostSets.Add(postSet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogBlogId = new SelectList(db.BlogSets, "BlogId", "Name", postSet.BlogBlogId);
            return View(postSet);
        }

        // GET: PostSets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostSet postSet = db.PostSets.Find(id);
            if (postSet == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogBlogId = new SelectList(db.BlogSets, "BlogId", "Name", postSet.BlogBlogId);
            return View(postSet);
        }

        // POST: PostSets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,Title,Content,BlogBlogId,CreatedDate")] PostSet postSet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postSet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogBlogId = new SelectList(db.BlogSets, "BlogId", "Name", postSet.BlogBlogId);
            return View(postSet);
        }

        // GET: PostSets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostSet postSet = db.PostSets.Find(id);
            if (postSet == null)
            {
                return HttpNotFound();
            }
            return View(postSet);
        }

        // POST: PostSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostSet postSet = db.PostSets.Find(id);
            db.PostSets.Remove(postSet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
