using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using RichnessSoft.Entity.Model;
using RichnessSoft.Service.BS;
using RichnessSoft.Web2.Pages.Databases.Products;

namespace RichnessSoft.Web2.Pages.Databases.Customers
{
    public partial class SaleAreas
    {
        [Parameter]
        public string ParrentMenu { get; set; }

        private bool _loaded;
        string backURL = "";
        List<SaleArea> ListData = new List<SaleArea>();
        private string _searchString { get; set; }
        private SaleArea _saleArea { get; set; }
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

            var res = await Task.Run(() => saleareaService.GetAllAsync(store.CurentCompany.id));
            ListData = (List<SaleArea>)res.Data;
        }
        protected override void OnParametersSet()
        {
            backURL = "SubMenu/" + ParrentMenu;
        }
        async void AddNewAsync()
        {
            string URL = $"/Database/TerritoryEdit/0/{ParrentMenu}";
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
            string URL = $"/Database/TerritoryEdit/{id}/{ParrentMenu}";
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
                var r = saleareaService.GetById(id);
                SaleArea salearea = (SaleArea)r.Data;
                var res = saleareaService.Delete(salearea);
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
        private bool Search(SaleArea salearea)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (salearea.code?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (salearea.name1?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (salearea.name2?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}