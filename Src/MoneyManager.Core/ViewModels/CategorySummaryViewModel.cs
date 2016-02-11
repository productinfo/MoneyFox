﻿using System.Collections.ObjectModel;
using MoneyManager.Core.StatisticProvider;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class CategorySummaryViewModel : StatisticViewModel
    {
        private readonly CategorySummaryDataProvider categorySummaryDataDataProvider;

        public CategorySummaryViewModel(IPaymentRepository paymentRepository, IRepository<Category> categoryRepository)
        {
            categorySummaryDataDataProvider = new CategorySummaryDataProvider(paymentRepository, categoryRepository);
        }

        /// <summary>
        ///     Loads the data with the current start and end date.
        /// </summary>
        public MvxCommand LoadCommand => new MvxCommand(Load);

        private void Load()
        {
            CategorySummary = null;
            CategorySummary = GetCategorySummaryData();
        }

        /// <summary>
        ///     Returns the Category Summary
        /// </summary>
        public ObservableCollection<StatisticItem> CategorySummary { get; set; }

        private ObservableCollection<StatisticItem> GetCategorySummaryData()
        {
            return new ObservableCollection<StatisticItem>(categorySummaryDataDataProvider.GetValues(StartDate, EndDate));
        }
    }
}
