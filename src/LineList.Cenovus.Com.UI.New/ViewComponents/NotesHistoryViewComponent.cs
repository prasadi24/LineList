using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.ViewComponents
{
    public class NotesHistoryViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;

        public NotesHistoryViewComponent(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}