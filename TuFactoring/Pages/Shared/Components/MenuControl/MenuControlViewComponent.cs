using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringModels;
using TuFactoring.Services;
using Microsoft.AspNetCore.Http;


namespace TuFactoring.Pages.Shared.Components.MenuControl
{
    public class MenuControlViewComponent : ViewComponent
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;
        private readonly IPeopleService _pS;
        private List<Menu> Menues { get; set; }

        public class paramMenu
        {
            public IList<MenuViewModel> menuModel { get; set; }

            public bool haveClients { get; set; }

            public bool isFintech { get; set; }

            public string token { get; set; }

            public IPeopleService pS { get; set; }
        }

        public MenuControlViewComponent(SignInManager<User> signInManager, UserManager<User> userManager, IAuthService authService, IPeopleService pS)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
            _pS = pS;
        }

        public async Task<IViewComponentResult> InvokeAsync(string participant)
        {
            paramMenu menu = new paramMenu();
            var menues = await _authService.GetUserMenu(participant);
            //menu.menuModel = GetMenu(menues, Guid.Parse("00000000-0000-0000-0000-000000000000"));

            menu.haveClients = false;

            menu.token = HttpContext.Session.GetString("token");

            menu.pS = _pS;

            return View("Default", menu);
        }

        private IList<MenuViewModel> GetMenu(IList<Menu> menuList, Guid? parentId)
        {
            var children = GetChildrenMenu(menuList, parentId);

            if (!children.Any())
            {
                return new List<MenuViewModel>();
            }

            var vmList = new List<MenuViewModel>();
            foreach (var item in children)
            {
                var menu = GetMenuItem(menuList, item.ID);
                var vm = new MenuViewModel();
                vm.ID = menu.ID;
                vm.Participant = menu.Participant;
                vm.ItemType = menu.ItemType;
                vm.Content = menu.Content;
                vm.Description = menu.Description;
                vm.IconClass = menu.IconClass;
                vm.Url = menu.Url;
                vm.SelectedStyle = menu.SelectedStyle;
                vm.SelectedClass = menu.SelectedClass;
                vm.OnClick = menu.OnClick;
                vm.Children = GetMenu(menuList, menu.ID);
                vmList.Add(vm);
            }
            return vmList;
        }

        private IList<Menu> GetChildrenMenu(IList<Menu> menuList, Guid? parentId = null)
        {
            return menuList.Where(x => x.ParentID == parentId).OrderBy(x => x.Order).ToList();
        }

        private Menu GetMenuItem(IList<Menu> menuList, Guid id)
        {
            return menuList.FirstOrDefault(x => x.ID == id);
        }
    }
}
