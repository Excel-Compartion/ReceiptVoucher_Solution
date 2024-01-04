using Microsoft.AspNetCore.Components;

namespace ReceiptVoucher.Server.Components.Pages
{
    public partial class BranchList
    {
        [Inject] private IUnitOfWork _IUnitOfWork { get; set; }
        [Inject] private IDialogService _DialogService { get; set; }
        [Inject] private ISnackbar _Snackbar { get; set; }

        public List<Branch?> Branches = null;

        //private NavigationManager _NavigationManager;

        public BranchList()
        {
            
        }

        protected override async Task OnInitializedAsync()
        {
            Branches = await _IUnitOfWork.Branches.GetAllAsync();
        }

        private bool FilterFunc1(Branch branch) => FilterFunc(branch, searchString1);

        private bool FilterFunc(Branch branch, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (branch.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }


    //<!--==================     Add / Edit Dialog      =========================================-->
    private void OpenAddDialog()
        {
            var parameters = new DialogParameters();
            parameters.Add("OnClose", EventCallback.Factory.Create<bool>(this, RefreshData));

            _DialogService.Show<_Branch_Add_Dialog>("اضافة فرع جديد", parameters);
        }

        private void OpenEditDialog(Branch branch)
        {
            var parameters = new DialogParameters();
            parameters.Add("OnClose", EventCallback.Factory.Create<bool>(this, RefreshData));
            parameters.Add("OriginalBranch", branch);

            _DialogService.Show<_Branch_Add_Dialog>("تعديل الفرع", parameters);
        }



        private async void RefreshData(bool dialogResult)
        {
            if (dialogResult)
            {
                Branches = await _IUnitOfWork.Branches.GetAllAsync();
                StateHasChanged();
            }
        }


        private async Task DeleteBranch(int branchId)
        {
            Branch? selectedBranch = Branches.SingleOrDefault(b => b.Id == branchId);

            if (selectedBranch != null)
            {
                var parameters = new DialogParameters() { { "BranchName", selectedBranch.Name } };
                var dialog = _DialogService.Show<_ConfirmDeleteDialog>("الرجاء التأكيد", parameters);

                var result = await dialog.Result;

                if (!result.Cancelled && result.Data is bool data && data)
                {
                    // User confirmed deletion, proceed with deletion

                    // Try to delete the branch

                    bool deleteResult = await _IUnitOfWork.Branches.DeleteAsync(branchId);

                    if (deleteResult)
                    {
                        // Deletion was successful
                        Branches.Remove(selectedBranch);

                        // Show a success message
                        _Snackbar.Add("تم حذف بيانات الفرع بنجاح", Severity.Success);
                    }
                    else
                    {
                        // Deletion failed
                        _Snackbar.Add("حدث خطاء اثناء حذف بيانات الفرع!", Severity.Error);
                    }

                    StateHasChanged();
                }
            }

        }

    }
}
