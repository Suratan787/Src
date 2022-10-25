using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using RichnessSoft.Entity.Model;

namespace RichnessSoft.Web2.Pages.Databases.Organizations
{
    public partial class Sections
    {
        [Parameter]
        public string ParrentMenu { get; set; }

        private bool _loaded;
        string backURL = "";
        List<Section> ListData = new List<Section>();
        private string _searchString { get; set; }
        private Section _section { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _loaded = true;
            await CheckState();
            Task.Delay(1000);
            await LoadData();
            _loaded = false;
        }
        async Task LoadData()
        {

            if (store.CurentCompany == null)
                return;

            var res = await Task.Run(() => sectionService.GetAllAsync(store.CurentCompany.id));
            ListData = (List<Section>)res.Data;
        }
        protected override void OnParametersSet()
        {
            backURL = "SubMenu/" + ParrentMenu;
        }

        async void AddNewAsync()
        {
            string URL = $"/Database/SectionsEdit/0/{ParrentMenu}";
            NavigationManager.NavigateTo(URL);
        }

        async void ReloadAsync()
        {
            _loaded = true;
            await LoadData();
            _loaded = false;
            StateHasChanged();
        }

        async void OnEdit(int id)
        {
            string URL = $"/Database/SectionsEdit/{id}/{ParrentMenu}";
            NavigationManager.NavigateTo(URL);
        }

        async void OnDelete(int id)
        {
            bool? result = await Dialog.ShowMessageBox(
           "Deletd", Lng["CONFIRM_MSG_DEL"],
           yesText: "Yes", cancelText: "Cancel");

            if (result == true)
            {
                _loaded = true;
                var r = sectionService.GetById(id);
                Section section = (Section)r.Data;
                var res = sectionService.Delete(section);
                if (res.Success)
                {
                    await Dialog.ShowMessageBox("info", Lng["CONFIRM_MSG_DEL_SUCCESS"], "OK");
                    await LoadData();
                }
                else
                {
                    await Dialog.ShowMessageBox("info", Lng["CONFIRM_MSG_DEL_FAIL"], "OK");
                    await Dialog.ShowMessageBox("info", res.Message, "OK");
                }
                _loaded = false;
                StateHasChanged();
            }
        }

        private bool Search(Section section)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (section.code?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (section.name1?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (section.name2?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }


    }
}