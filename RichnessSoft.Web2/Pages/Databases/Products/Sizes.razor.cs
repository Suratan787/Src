using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using RichnessSoft.Entity.Model;
using RichnessSoft.Web2.Pages.Databases.Products;

namespace RichnessSoft.Web2.Pages.Databases.Products
{
    public partial class Sizes
    {
        [Parameter]
        public string ParrentMenu { get; set; }

        private bool _loaded;
        string backURL = "";
        List<Sizex> ListData = new List<Sizex>();
        private string _searchString { get; set; }
        private Sizex _sizex { get; set; }
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

            var res = await Task.Run(() => sizeService.GetAllAsync(store.CurentCompany.id));
            ListData = (List<Sizex>)res.Data;
        }
        protected override void OnParametersSet()
        {
            backURL = "SubMenu/" + ParrentMenu;
        }
        async void AddNewAsync()
        {
            string URL = $"/Database/SizesEdit/0/{ParrentMenu}";
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
            string URL = $"/Database/SizesEdit/{id}/{ParrentMenu}";
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
                var r = sizeService.GetById(id);
                Sizex sizex = (Sizex)r.Data;
                var res = sizeService.Delete(sizex);
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
        private bool Search(Sizex sizex)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (sizex.code?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (sizex.name1?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (sizex.name2?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}