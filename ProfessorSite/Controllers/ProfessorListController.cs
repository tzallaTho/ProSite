using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using ModelEntityFrm;
using PagedList;
using PagedList.Mvc;

namespace ProfessorSite.Controllers
{
    public class ProfessorListController : Controller
    {
        private const int PageSize = 3;
        //
        // GET: /ProfessorList/

        public ActionResult Index(int? page)
        {
              var listPaged = GetPagedNames(page); // GetPagedNames is found in BaseController
            if (listPaged == null)
                return HttpNotFound();

            // pass the paged list to the view and render
            ViewBag.Names = listPaged;
            return View();
        }

        public ActionResult GetData(string sortOrder, string currentFilter, string searchByName,
            string searchByDescription, string searchByUser, string searchByBrand,
            string searchByCategory, string StateList, int? page)
        {

            if (searchByName != null)
            {
                page = 1;
            }
            else
            {
                searchByName = currentFilter;
            }

            // var efmvcUser = HttpContext.User.GetEFMVCUser();
            // var user = _userRepository.GetById(efmvcUser.UserId);

            // SetCollectionForFilter(user);

            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
              //ViewBag.DescriptionSortParm = sortOrder == "UserName" ? "DescriptionDesc" : "Description";
            //  ViewBag.UserSortParm = sortOrder == "User" ? "UserDesc" : "User";
            // ViewBag.PriceSortParm = sortOrder == "Price" ? "PriceDesc" : "Price";
            // ViewBag.CategorySortParm = sortOrder == "Category" ? "CategoryDesc" : "Category";
            //  ViewBag.BrandSortParm = sortOrder == "Brand" ? "BrandDesc" : "Brand";

            ViewBag.CurrentFilter = searchByName;
            ViewBag.DescriptionFilter = searchByDescription;
            ViewBag.UserParam = searchByUser;
            ViewBag.BrandParam = searchByBrand;
            ViewBag.CategoryParm = StateList;      

            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            // filter
            var dd = new ProfessorContext();


            var categories = new List<string>();
            var categoriesQuery = from cc in dd.states
                                  orderby cc.description
                                  select cc.description;
            categories.AddRange(categoriesQuery.Distinct());
            ViewBag.StateList = new SelectList(categories);


            IList<ProfessorClass> products = dd.professors.ToList();
            IList<InformationClass> info = dd.users.ToList();
            IList<State> ss = dd.states.ToList();

            products.ToList().ForEach(p => p.info = info.First(h => h.id == p.info.id));
            products.ToList().ForEach(p => p._state = ss.First(h => h.id == p._state.id));

            //_productRepository.GetMany(b => b.CompanyId == user.CompanyId).ToList();
            if (!String.IsNullOrEmpty(searchByName))
            {
                products = products.Where(b => b.info.name.ToUpper().Contains(searchByName.ToUpper()) || b.info.UserName.ToUpper().Contains(searchByName.ToUpper())).ToList();
            }


            if (!string.IsNullOrEmpty(StateList))
            {
                //int sid = int.Parse(StateList);
                products = products.Where(x => x._state.description == StateList).ToList();
            }

            // Sort
            IPagedList<ProfessorClass> productsToReturn = null;
           switch (sortOrder)
            {
                case "NameDesc":
                    productsToReturn = products.OrderByDescending(b => b.info.surname).ToPagedList(pageIndex, PageSize);
                    break;
                case "User":
                    productsToReturn = products.OrderBy(b => b.info.surname).ToPagedList(pageIndex, PageSize);
                    break;
               default:
                   break;
           }

           if (productsToReturn == null)
           {
               productsToReturn = products.OrderBy(b => b.info.UserName).ToPagedList(pageIndex, PageSize);
           }

            return View(productsToReturn);
        }

        protected IEnumerable<string> GetStuffFromDatabase()
        {
            var sampleData = new StreamReader(Server.MapPath("~/App_Data/Names2.txt")).ReadToEnd();
            return sampleData.Split('\n');
        }

        protected IPagedList<string> GetPagedNames(int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
                return null;

            // retrieve list from database/whereverand
            var listUnpaged = GetStuffFromDatabase();

            // page the list
            const int pageSize = 20;
            var listPaged = listUnpaged.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }
    }
}

