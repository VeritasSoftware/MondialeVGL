using CsvHelper;
using CsvHelper.Configuration;
using MondialeVGL.OrderProcessor.Repository.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace MondialeVGL.OrderProcessor.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _orderFilePath;

        public OrderRepository(string orderFilePath)
        {
            _orderFilePath = orderFilePath;
        }

        public async IAsyncEnumerable<OrderEntity> GetOrdersAsync()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            using (var reader = new StreamReader(_orderFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                bool isNewOrder = true;
                OrderEntity currentOrder = null;

                while (await csv.ReadAsync())
                {
                    isNewOrder = string.Compare(csv.GetField(0), RecordType.H.ToString(), true) == 0;

                    if (isNewOrder)
                    {
                        if (currentOrder != null)
                        {
                            yield return currentOrder;
                        }

                        currentOrder = new OrderEntity();

                        var orderHeader = csv.GetRecord<OrderHeaderEntity>();

                        currentOrder.Header = orderHeader;
                        currentOrder.Details = new List<OrderDetailEntity>();
                    }
                    else
                    {
                        var orderDetail = csv.GetRecord<OrderDetailEntity>();

                        currentOrder.Details.Add(orderDetail);
                    }
                }

                if (currentOrder != null)
                {
                    yield return currentOrder;
                }
            }

            await Task.CompletedTask;
        }
    }
}
