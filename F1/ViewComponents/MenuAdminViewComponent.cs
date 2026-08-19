using F1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace F1.ViewComponents
{
	public class MenuAdminViewComponent : ViewComponent
	{
		private IRepositoryF1 repo;

		public MenuAdminViewComponent(IRepositoryF1 repo)
		{
			this.repo = repo;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
