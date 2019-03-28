using coliks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace coliks.Controllers
{
    public class FilterDataController : Controller
    {

        //This is the common function used for paging and searching  
        public GridPagination FilterData(int? PageNumber, FilterItems filters, ColiksContext context)
        {
            GridPagination gridData = new GridPagination();
            double count = 0;
            try
            {
                using (ColiksContext db = new ColiksContext())
                {
                    // Getting all the Data from Database  
                    var empData = db.Items.Include(i => i.Category).ToList();
                    // Checking if the Page number is passed and is greater than 0 else considered as 1  
                    gridData.CurrentPage = PageNumber.HasValue ? PageNumber.Value <= 0 ? 1 : PageNumber.Value : 1;
                    // Assigning the list of data to the Model's property  
                    gridData.Data = empData;
                    // Assigning filters   
                    gridData.filters = filters;

                    //Getting the List with the matching search  
                    if (!string.IsNullOrEmpty(filters.search))
                    {
                        gridData.Data = gridData.Data.Where(x => !string.IsNullOrEmpty(x.Brand)).ToList(); // Have to check if the brand is null or empty, to avoid a nullErrorExeptions on .ToLower()
                        gridData.Data = gridData.Data.Where(x => !string.IsNullOrEmpty(x.Model)).ToList(); // Have to check if the model is null or empty, to avoid a nullErrorExeptions on .ToLower()

                        gridData.Data = gridData.Data.Where(x =>
                        x.Itemnb.ToLower().Contains(filters.search.ToLower())  ||
                        x.Brand.ToLower().Contains(filters.search.ToLower())   ||
                        x.Model.ToLower().Contains(filters.search.ToLower())   ||
                        x.Stock.ToString().Contains(filters.search.ToLower())
                        ).ToList();
                    }

                    //Getting the List with the matching itemnb  
                    if (!string.IsNullOrEmpty(filters.itemnb))
                    {
                        gridData.Data = gridData.Data.Where(x => x.Itemnb.ToLower().Contains(filters.itemnb.ToLower())).ToList();
                    }

                    //Getting the List with the matching brand 
                    if (!string.IsNullOrEmpty(filters.brand))
                    {
                        gridData.Data = gridData.Data.Where(x => !string.IsNullOrEmpty(x.Brand)).ToList(); // Have to check if the brand is null or empty, to avoid a nullErrorExeptions on .ToLower()
                        gridData.Data = gridData.Data.Where(x => x.Brand.ToLower().Contains(filters.brand.ToLower())).ToList();
                    }
                    
                    //Getting the List with the matching model  
                    if (!string.IsNullOrEmpty(filters.model))
                    {
                        gridData.Data = gridData.Data.Where(x => !string.IsNullOrEmpty(x.Model)).ToList(); // Have to check if the model is null or empty, to avoid a nullErrorExeptions on .ToLower()
                        gridData.Data = gridData.Data.Where(x => x.Model.ToLower().Contains(filters.model.ToLower())).ToList();
                    }

                    //Getting the List with the matching size
                    if (!string.IsNullOrEmpty(filters.size.ToString()))
                    {
                        gridData.Data = gridData.Data.Where(x => !string.IsNullOrEmpty(x.Size.ToString())).ToList(); // Have to check if the size is null or empty, to avoid a nullErrorExeptions on .Contains()
                        gridData.Data = gridData.Data.Where(x => x.Size.ToString().Contains(filters.size.ToString())).ToList();
                    }

                    //Getting the List with the matching stock  
                    if (!string.IsNullOrEmpty(filters.stock.ToString()))
                    {
                        gridData.Data = gridData.Data.Where(x => x.Stock.ToString().Contains(filters.stock.ToString())).ToList();
                    }

                    //Getting the List with the matching category  
                    if (filters.category != null)
                    {
                        var category = context.Categories.Find(filters.category);
                        gridData.Data = gridData.Data.Where(x => x.Category.Description.ToLower().Contains(category.Description.ToLower())).ToList();
                    }

                    // If there are multiple filter key passed then the above condition will work as an operator condition  

                    // Total data count after filter  
                    gridData.TotalData = gridData.Data.Count();

                    // Getting the total pages   
                    count = (double)gridData.TotalData / gridData.TakeCount;
                    gridData.TotalPage = (int)Math.Ceiling(count);

                    // assigning the filtered data to model  
                    // This is the formula for skiping the previous page's data and taking the current page's  
                    gridData.Data = gridData.Data.Skip((gridData.CurrentPage - 1) * gridData.TakeCount).Take(gridData.TakeCount).ToList();
                }
            }
            catch (Exception ex)
            {
                gridData = new GridPagination();
            }
            //returning the Grid.  
            return gridData;
        }
    }
}
    
    



