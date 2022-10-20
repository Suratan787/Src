using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using RichnessSoft.Entity.Model;

namespace RichnessSoft.Web2.Pages.Databases.Products
{
    public partial class Formats
    {
        [Parameter]
        public string ParrentMenu { get; set; }

        private bool _loaded;
        string backURL = "";
        List<Format> ListData = new List<Format>();
        private string _searchString { get; set; }
        private Format _format { get; set; }

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

            var res = await Task.Run(() => formatService.GetAllAsync(store.CurentCompany.id));
            ListData = (List<Format>)res.Data;
        }
        protected override void OnParametersSet()
        {
            backURL = "SubMenu/" + ParrentMenu;
        }

        async void AddNewAsync()
        {
            string URL = $"/Database/FormatEdit/0/{ParrentMenu}";
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
            string URL = $"/Database/FormatEdit/{id}/{ParrentMenu}";
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
                var r = formatService.GetById(id);
                Format formats = (Format)r.Data;
                var res = formatService.Delete(formats);
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

        private bool Search(Format formats)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (formats.code?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (formats.name1?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (formats.name2?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }


    }
}