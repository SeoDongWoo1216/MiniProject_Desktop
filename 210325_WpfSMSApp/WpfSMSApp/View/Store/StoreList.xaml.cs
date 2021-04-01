using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfSMSApp.View.Store
{
    /// <summary>
    /// MyAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StoreList : Page
    {
        public StoreList()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Store 테이블 데이터 읽어와야 함
                List<Model.Store> stores = new List<Model.Store>();  // 원본
                List<Model.StockStore> stockStores = new List<Model.StockStore>();
                List<Model.Stock> stocks = new List<Model.Stock>();

                stores = Logic.DataAccess.GetStores();
                stocks = Logic.DataAccess.GetStocks();

                // TODO : stores 데이터를 stockStores로 복사
                foreach (Model.Store item in stores)
                {
                    stockStores.Add(new Model.StockStore()
                    {
                        StoreID = item.StoreID,
                        StoreName = item.StoreName,
                        StoreLocation = item.StoreLocation,
                        ItemStatus = item.ItemStatus,
                        TagID = item.TagID,
                        BarcodeID = item.BarcodeID,
                        StockQuantity = 0
                    });
                }

                this.DataContext = stockStores;
            }

            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 StoreList Loaded : {ex}");
                throw ex;
            }
        }

        
        private void BtnDeactivateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               //  NavigationService.Navigate(new DeactiveUser());
            }

            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 BtnDeactivateUser_Click : {ex}");
                throw ex;
            }
        }

      

        private void BtnAddStore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new AddStore());
            }

            catch (Exception ex)
            {
                Commons.LOGGER.Error($"예외 발생 BtnAddStore_Click : {ex}");
                throw ex;
            }
        }

        private void BtnEditStore_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExportExcel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
