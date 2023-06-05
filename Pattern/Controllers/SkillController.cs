using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pattern.DTO;
using Pattern.Entity.DataModels;
using Pattern.Models;
using Pattern.Service.Interface;

namespace Pattern.Controllers
{
    public class SkillController : Controller
    {
        private readonly ISkillService _service;
        private readonly IMapper _mapper;

        public SkillController(ISkillService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            FilterParams filterParams = new FilterParams(1, 10, string.Empty, DateTime.MinValue, DateTime.UtcNow);
            PageList<SkillDTO> list = _service.GetList(filterParams);

            FilterParamsVM filtersVm = new FilterParamsVM();
            PageListVM<UpsertSkillVm> pageListVm = _mapper.Map<PageListVM<UpsertSkillVm>>(list);

            SkillPageVM skillPageVM = new SkillPageVM()
            {
                Filters = filtersVm,
                Skills= pageListVm
            };
            return View(skillPageVM);
        }


        [HttpPost]
        public IActionResult GetList(FilterParamsVM filterParams) 
        {
            FilterParams filters = _mapper.Map<FilterParams>(filterParams);
            PageList<SkillDTO> list = _service.GetList(filters);
            PageListVM<UpsertSkillVm> pageListVm = _mapper.Map<PageListVM<UpsertSkillVm>>(list);

            return PartialView("Skill/_SkillList", pageListVm);
        }

        //[HttpGet]
        //public async Task<IActionResult> Upsert(int id)
        //{
        //    Skill skill = await _service.FindByIdAsync(id);
        //    UpsertSkillVm skillVm = _mapper.Map<UpsertSkillVm>(skill);
        //    return Ok();
        //}


        [HttpPost]
        public async Task<IActionResult> Upsert(UpsertSkillVm skillVm)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "fill all details correctly";
                return RedirectToAction("Index");
            }
            Skill skill = _mapper.Map<Skill>(skillVm);
            if (skill.Id == 0)
            {
                skill.CreatedAt = DateTime.UtcNow;
                await _service.AddAsync(skill);
                TempData["success"] = "Skill added successfully!";
            }
            else
            {
                skill.UpdatedAt = DateTime.UtcNow;
                await _service.UpdateAsync(skill);
                TempData["success"] = "Skill updated successfully!";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            TempData["warning"] = "skill deleted!";
            return RedirectToAction("Index");
        }
        



    }
}
