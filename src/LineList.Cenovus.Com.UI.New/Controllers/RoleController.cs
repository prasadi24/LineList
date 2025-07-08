using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.Role;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
	public class RoleController : Controller
	{
		private readonly IRoleService _roleService;
		private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public RoleController(IRoleService roleService, IMapper mapper, CurrentUser currentUser)
		{
			_roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var roles = await _roleService.GetAll();
			var roleDtos = _mapper.Map<IEnumerable<RoleResultDto>>(roles);
			return View(roleDtos);
		}

		[HttpGet]
		public async Task<JsonResult> RoleFeed()
		{
			var roles = await _roleService.GetAll();
			var roleDtos = _mapper.Map<IEnumerable<RoleResultDto>>(roles);
			return Json(new { data = roleDtos });
		}

		[HttpGet]
		public IActionResult Create()
		{
			var model = new RoleAddDto
			{
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
			return PartialView("_Create", model);
		}

		[HttpPost]
		public async Task<JsonResult> Create(RoleAddDto model)
		{
			if (!ModelState.IsValid)
				return Json(new { success = false, ErrorMessage = "Model is not valid" });

			var role = _mapper.Map<Role>(model);
			await _roleService.Add(role);

			return Json(new { success = true });
		}

		[HttpGet]
		public async Task<IActionResult> Update(Guid id)
		{
			var role = await _roleService.GetById(id);
			if (role == null)
				return NotFound();

			var roleDto = _mapper.Map<RoleEditDto>(role);
			return PartialView("_Update", roleDto);
		}

		[HttpPost]
		public async Task<JsonResult> Update(RoleEditDto model)
		{
			if (!ModelState.IsValid)
				return Json(new { success = false, ErrorMessage = "Model is not valid" });

			var role = _mapper.Map<Role>(model);
			await _roleService.Update(role);

			return Json(new { success = true });
		}

		[HttpDelete]
		public async Task<JsonResult> Delete(Guid id)
		{
			var role = await _roleService.GetById(id);
			if (role == null)
				return Json(new { success = false, ErrorMessage = "Role not found" });

			await _roleService.Remove(role);
			return Json(new { success = true });
		}
	}
}