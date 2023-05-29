using AutoMapper;
using Pattern.DTO;
using Pattern.Entity.DataModels;
using Pattern.Repository.Interface;
using Pattern.Service.Interface;

namespace Pattern.Service
{
    public class SkillService : BaseService<Skill>, ISkillService
    {
        private readonly ISkillRepo _repo;
        private readonly IMapper _mapper;

        public SkillService(ISkillRepo repo, IMapper mapper):base(repo) 
        {
            _repo = repo;
            _mapper = mapper;
        }


        public PageList<SkillDTO> GetList(FilterParams filterParams)
        {
            IQueryable<Skill> query = _repo.GetAllAsync();

            switch (filterParams.Status)
            {
                case 0:
                    break;
                case 1:
                    query = query.Where(q => q.Status == true);
                    break;
                case 2:
                    query = query.Where(q => q.Status == false);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(filterParams.ContainSearch))
            {
                query = query.Where(q => q.Name.ToLower().Contains(filterParams.ContainSearch.ToLower()));
            }

            if(filterParams.StartDate != DateTime.MinValue)
            {
                query = query.Where(q => q.CreatedAt >= filterParams.StartDate);
            }

            if(filterParams.EndDate != DateTime.MinValue)
            {
                query = query.Where(q => q.CreatedAt.Date <= filterParams.EndDate.Date);
            }

            switch (filterParams.SortCol)
            {
                case "created_at":
                    query = filterParams.SortDesc ? query.OrderByDescending(q => q.CreatedAt) : query.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    query = filterParams.SortDesc ? query.OrderByDescending(q => q.Name.ToLower()) : query.OrderBy(q => q.Name.ToLower());
                    break;
            }
            
            var totalRecords = query.Count();

            var records = query.Skip((filterParams.PageIndex-1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToList();

            List<SkillDTO> list = _mapper.Map<List<SkillDTO>>(records);

            return new PageList<SkillDTO>(filterParams.PageIndex, filterParams.PageSize, totalRecords, list);
        }

    }
}
