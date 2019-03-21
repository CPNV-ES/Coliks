using coliks.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace coliks.Controllers
{
    public class FilterDataController : Controller
    {
        //This is the common function used for paging and searching  
        public GridPagination FilterData(int? PageNumber, FilterItems filters)
        {
            GridPagination gridData = new GridPagination();
            double count = 0;
            try
            {
                using (ColiksContext db = new ColiksContext())
                {
                    // Getting all the Data from Database  
                    var empData = db.Items.ToList();
                    // Checking if the Page number is passed and is greater than 0 else considered as 1  
                    gridData.CurrentPage = PageNumber.HasValue ? PageNumber.Value <= 0 ? 1 : PageNumber.Value : 1;
                    // Assigning the list of data to the Model's property  
                    gridData.Data = empData;
                    // Assigning filters   
                    gridData.filters = filters;

                    //Getting the List with the matching itemnb  
                    if (!string.IsNullOrEmpty(filters.itemnb))
                    {
                        gridData.Data = gridData.Data.Where(x => x.Itemnb.ToLower().Contains(filters.itemnb.ToString())).ToList();
                    }

                    //Getting the List with the matching brand  
                    if (!string.IsNullOrEmpty(filters.brand))
                    {
                        gridData.Data = gridData.Data.Where(x => x.Brand != null && x.Brand.ToLower().Contains(filters.brand.ToString())).ToList();
                    }
                    
                    //Getting the List with the matching model  
                    if (!string.IsNullOrEmpty(filters.model))
                    {
                        gridData.Data = gridData.Data.Where(x => !string.IsNullOrEmpty(x.Model) && x.Model.Contains(filters.model)).ToList();

                        //var tmpGrid = new List<Items>();
                        //foreach(var e in gridData.Data)
                        //{
                        //    if (!string.IsNullOrEmpty(e.Model))
                        //    {
                        //        if (e.Model.Contains(filters.model))
                        //        {
                        //            tmpGrid.Add(e);
                        //        }
                        //    }
                        //}
                        //gridData.Data = tmpGrid;
                    }

                    //Getting the List with the matching size  
                    if (!string.IsNullOrEmpty(filters.size.ToString()))
                    {
                        gridData.Data = gridData.Data.Where(x => x.Size.ToString().Contains(filters.size.ToString())).ToList();
                    }

                    //Getting the List with the matching stock  
                    if (!string.IsNullOrEmpty(filters.stock.ToString()))
                    {
                        gridData.Data = gridData.Data.Where(x => x.Stock.ToString().Contains(filters.stock.ToString())).ToList();
                    }

                    //Getting the List with the matching category  
                    if (!string.IsNullOrEmpty(filters.category))
                    {
                        gridData.Data = gridData.Data.Where(x => x.Category.Description.ToString().ToLower().Contains(filters.category.ToLower())).ToList();
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
    
    



