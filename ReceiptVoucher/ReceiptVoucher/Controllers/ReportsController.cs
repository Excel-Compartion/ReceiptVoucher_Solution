using AspNetCore.ReportingServices.ReportProcessing.OnDemandReportObjectModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReceiptVoucher.Core.Enums;
using System.Linq.Expressions;

namespace ReceiptVoucher.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReportsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllBranchesBarChartDataAsync")]
        public async Task<IActionResult> GetAllBranchesBarChartDataAsync()
        {
            var branch = await _unitOfWork.Branches.GetAllAsync();

            if (branch == null || branch.Count() == 0) { return NotFound(); }

            // BranchesNames []

            int branchCount = branch.Count();

            string[] BranchesNames = new string[branchCount];

            for (int i = 0; i < branchCount; i++)
            {
                BranchesNames[i] = branch[i].Name;
            }

            var receipt = await _unitOfWork.Receipts.GetAllAsync();

            double[] Individual = new double[branchCount];
            double[] Company = new double[branchCount];
            double[] Association = new double[branchCount];
            double[] Foundation = new double[branchCount];

            if (receipt != null && receipt.Count() != 0)
            {
                for (int i = 0; i < branchCount; i++)
                {
                    decimal IndividualTotalAmount;
                    decimal CompanyTotalAmount;
                    decimal AssociationTotalAmount;
                    decimal FoundationTotalAmount;

                    IndividualTotalAmount = receipt.Where(x => x.BranchId == branch[i].Id && x.GrantDestinations == GrantDest.Individual && x.Date.Year==DateTime.Now.Year).Select(x => x.TotalAmount).Sum();
                    CompanyTotalAmount = receipt.Where(x => x.BranchId == branch[i].Id && x.GrantDestinations == GrantDest.Company && x.Date.Year == DateTime.Now.Year).Select(x => x.TotalAmount).Sum();
                    AssociationTotalAmount = receipt.Where(x => x.BranchId == branch[i].Id && x.GrantDestinations == GrantDest.Association && x.Date.Year == DateTime.Now.Year).Select(x => x.TotalAmount).Sum();
                    FoundationTotalAmount = receipt.Where(x => x.BranchId == branch[i].Id && x.GrantDestinations == GrantDest.Foundation && x.Date.Year == DateTime.Now.Year).Select(x => x.TotalAmount).Sum();

                    Individual[i] = Convert.ToDouble(IndividualTotalAmount);
                    Company[i] = Convert.ToDouble(CompanyTotalAmount);
                    Association[i] = Convert.ToDouble(AssociationTotalAmount);
                    Foundation[i] = Convert.ToDouble(FoundationTotalAmount);
                }
            }

            BranchesBarChartViewModel branchesBarChartViewModel = new BranchesBarChartViewModel();

            branchesBarChartViewModel.BranchesNames = BranchesNames;
            branchesBarChartViewModel.Individual = Individual;
            branchesBarChartViewModel.Company = Company;
            branchesBarChartViewModel.Association = Association;
            branchesBarChartViewModel.Foundation = Foundation;

            return Ok(branchesBarChartViewModel);
        }

        [HttpGet("GetAllProjectsLineChartDataAsync")]
        public async Task<IActionResult> GetAllProjectsLineChartDataAsync()
        {
            var Projects = await _unitOfWork.Projects.GetAllAsync();

            if (Projects == null || Projects.Count() == 0)
            {
                return NotFound();
            }

            var receipt = await _unitOfWork.Receipts.GetAllAsync();

            Dictionary<string, double[]> MainProjects = new Dictionary<string, double[]>();

            foreach (var project in Projects)
            {
                double[] TotalAmountForMonths = new double[12];

                for (int i = 0; i < 12; i++)
                {
                    var TotalAmount = receipt.Where(x => x.ProjectId == project.Id && (x.Date.Month == i+1 && x.Date.Year==DateTime.Now.Year)).Select(x => x.TotalAmount).Sum();

                    
                    TotalAmountForMonths[i] = Convert.ToDouble(TotalAmount);
                }

                MainProjects[project.Name] = TotalAmountForMonths;
            }

           

            return Ok(MainProjects);
        }



    }


}
