using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using RichnessSoft.Common;
using RichnessSoft.Entity.Model;
using RichnessSoft.Web2.Pages.Databases.Products;

namespace RichnessSoft.Web2.Pages.Databases.Customers
{
    public partial class SaleAreasEdit
    {
        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string ParrentMenu { get; set; }
        private bool _loaded;
        string backURL = "";
        string Mode { get; set; }
        SaleArea salearea { get; set; }
        MudDatePicker _picker;

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        protected override async Task OnInitializedAsync()
        {
            backURL = "/Database/Territory/" + ParrentMenu;
            if (Id > 0)
            {
                Mode = gbVar.ModeEdit;
                var r = saleareaService.GetById(Id);
                salearea = (SaleArea)r.Data;
            }
            else
            {
                Mode = gbVar.ModeInsert;
                salearea = new SaleArea();
                salearea.companyid = store.CurentCompany.id;
                salearea.active = ConstUtil.ACTIVE.YES;
            }
        }

        async void SaveAsync()
        {
            ResultModel results = new ResultModel();
            try
            {
                _loaded = true;
                string strErrMsg = "";
                string strErrFocus = "";
                if (Validated && CheckDupCode())
                {
                    if (Mode == gbVar.ModeInsert)
                    {
                        results = saleareaService.Add(salearea);
                    }
                    else if (Mode == gbVar.ModeEdit)
                    {
                        results = saleareaService.Edit(salearea);
                    }
                    _loaded = false;
                    if (results.Success)
                    {
                        await Dialog.ShowMessageBox("info", Lng["SAVE_MSG_SUCCESS"], "OK");
                        if (Mode == gbVar.ModeInsert)
                        {
                            salearea = new SaleArea();
                        }
                        else
                        {
                            NavigationManager.NavigateTo($"/Database/Territory/{ParrentMenu}");
                        }
                    }
                    else
                    {
                        await Dialog.ShowMessageBox("error", Lng["SAVE_MSG_FAIL"], "OK");
                        await Dialog.ShowMessageBox("info", results.Message, "OK");
                    }
                    StateHasChanged();
                }
                _loaded = false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool CheckDupCode()
        {
            bool bSucc = true;
            var res = saleareaService.GetByCode(salearea.companyid, salearea.code);
            if (res.Data != null && !string.IsNullOrEmpty(((SaleArea)res.Data)?.code))
            {
                SaleArea OldData = (SaleArea)res.Data;
                if (Mode == gbVar.ModeInsert)
                {
                    bSucc = false;
                }
                else if (Mode == gbVar.ModeEdit && OldData.id != salearea.id)
                {
                    bSucc = false;
                }
            }
            if (bSucc == false)
            {
                _snackBar.Add(Lng["DUPLICATED_CODE"], Severity.Error);
            }
            return bSucc;
        }


        async void activeChange(IEnumerable<string> values)
        {
            var sz = values.ToArray();
            if (sz[0] == ConstUtil.ACTIVE.YES)
            {
                salearea.inactivedate = null;
                _picker.Clear();
            }
            StateHasChanged();
        }

    }
}