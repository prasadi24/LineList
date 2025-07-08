using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.UI.New.Controllers
{
    public class ValidationController : Controller
    {
        LineListDbContext _db;
        public ValidationController(LineListDbContext db)
        {
            _db = db;
        }
        [HttpPost]
        public async Task<JsonResult> IsNameUnique([FromBody] NameUniqueRequest request)
        {
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.TableName))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var dbSetProperty = _db.GetType()
                .GetProperties()
                .FirstOrDefault(p => p.Name.Equals(request.TableName, StringComparison.OrdinalIgnoreCase));

            if (dbSetProperty == null)
                return Json(new { success = false, ErrorMessage = $"Table '{request.TableName}' not found." });

            var dbSet = dbSetProperty.GetValue(_db);

            // Use LINQ.Dynamic.Core to query by name
            var queryable = dbSet as IQueryable<object>;
            if (queryable == null)
                return Json(new { success = false, ErrorMessage = "Invalid table query." });

            var exists = queryable.Any(e => EF.Property<string>(e, "Name") == request.Name);

            return Json(new { success = !exists });
        }

        [HttpPost]
        public async Task<JsonResult> IsSortOrderUnique([FromBody] NameUniqueRequest request)
        {
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.TableName))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var dbSetProperty = _db.GetType()
                .GetProperties()
                .FirstOrDefault(p => p.Name.Equals(request.TableName, StringComparison.OrdinalIgnoreCase));

            if (dbSetProperty == null)
                return Json(new { success = false, ErrorMessage = $"Table '{request.TableName}' not found." });

            var dbSet = dbSetProperty.GetValue(_db);

            // Use LINQ.Dynamic.Core to query by name
            var queryable = dbSet as IQueryable<object>;
            if (queryable == null)
                return Json(new { success = false, ErrorMessage = "Invalid table query." });

            var exists = queryable.Any(e => EF.Property<string>(e, "SortOrder") == request.Name);

            return Json(new { success = !exists });
        }

    }
}
