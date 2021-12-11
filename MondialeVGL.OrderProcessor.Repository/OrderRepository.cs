using CsvHelper;
using CsvHelper.Configuration;
using MondialeVGL.OrderProcessor.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace MondialeVGL.OrderProcessor.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _orderFilePath;

        public static event Func<Exception, Task> OnReadError;

        public OrderRepository(string orderFilePath)
        {
            _orderFilePath = orderFilePath;
        }

        public async Task<OrderCollectionEntity> GetOrdersAsync()
        {
            var orders = new OrderCollectionEntity
            {
                Orders = new List<OrderEntity>()
            };            

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };            

            using (var reader = new StreamReader(_orderFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                bool isNewOrder = true;
                OrderEntity currentOrder = null;

                while (await csv.ReadAsync())
                {
                    try
                    {
                        var strRecordType = csv.GetField(0);

                        if (string.IsNullOrEmpty(strRecordType) ||
                            !(string.Compare(strRecordType, RecordType.H.ToString(), true) == 0 ||
                            string.Compare(strRecordType, RecordType.D.ToString(), true) == 0))
                        {
                            throw new ApplicationException($"{strRecordType} is an invalid record type. Valid values [H|D].");
                        }

                        isNewOrder = string.Compare(strRecordType, RecordType.H.ToString(), true) == 0;
                    }
                    catch(Exception ex)
                    {
                        OnReadError?.Invoke(ex);
                        isNewOrder = false;
                        currentOrder = null;
                        continue;
                    }

                    if (isNewOrder)
                    {
                        if (currentOrder != null)
                        {
                            orders.Orders.Add(currentOrder);
                        }

                        currentOrder = new OrderEntity();
                        currentOrder.Details = new List<OrderDetailEntity>();

                        try
                        {
                            var orderHeader = csv.GetRecord<OrderHeaderEntity>();

                            currentOrder.Header = orderHeader;                            
                        }
                        catch(Exception ex)
                        {
                            OnReadError?.Invoke(ex);
                            currentOrder = null;
                            continue;
                        }
                    }
                    else if (!isNewOrder && currentOrder != null && currentOrder.Header != null)
                    {
                        try
                        {
                            var orderDetail = csv.GetRecord<OrderDetailEntity>();

                            currentOrder.Details.Add(orderDetail);
                        }
                        catch (Exception ex)
                        {
                            OnReadError?.Invoke(ex);
                            continue;
                        }                      
                    }
                }

                if (currentOrder != null)
                {
                    orders.Orders.Add(currentOrder);
                }
            }

            return orders;
        }
    }
}
