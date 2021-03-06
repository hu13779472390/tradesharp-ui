/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* TradeSharp is a C# based data feed and broker neutral Algorithmic 
* Trading Platform that lets trading firms or individuals automate 
* any rules based trading strategies in stocks, forex and ETFs. 
* TradeSharp allows users to connect to providers like Tradier Brokerage, 
* IQFeed, FXCM, Blackwood, Forexware, Integral, HotSpot, Currenex, 
* Interactive Brokers and more. 
* Key features: Place and Manage Orders, Risk Management, 
* Generate Customized Reports etc 
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using NUnit.Framework;
using Spring.Context.Support;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHub.StrategyEngine.OrderExecution;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;
using OrderExecutionProvider = TradeHubGui.Common.Models.OrderExecutionProvider;
using TradeHubConstants = TradeHub.Common.Core.Constants;

namespace TradeHubGui.Dashboard.Tests.Integration
{
    [TestFixture]
    public class OrderExecutionControllerTests
    {
        private OrderExecutionController _orderExecutionController;

        [SetUp]
        public void StartUp()
        {
            _orderExecutionController = ContextRegistry.GetContext()["OrderExecutionController"] as OrderExecutionController;
        }

        [TearDown]
        public void TearDown()
        {
            _orderExecutionController.Stop();
        }

        [Test]
        [Category("Integration")]
        public void RequestNewConnection_SendRequestToServer_ReceiveLogon()
        {
            Thread.Sleep(9000);
            OrderExecutionProvider provider = new OrderExecutionProvider(Dispatcher.CurrentDispatcher);
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = TradeHubConstants.OrderExecutionProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<OrderExecutionProvider>(provider);

            Thread.Sleep(9000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));
        }

        [Test]
        [Category("Console")]
        public void RequestNewMarketOrder_SendRequestToServer_ReceiveOrderAcceptance()
        {
            Thread.Sleep(5000);
            OrderExecutionProvider provider = new OrderExecutionProvider(Dispatcher.CurrentDispatcher);
            provider.ProviderType = ProviderType.OrderExecution;
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = TradeHubConstants.OrderExecutionProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<OrderExecutionProvider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));

            OrderDetails orderDetails = new OrderDetails(TradeHubConstants.OrderExecutionProvider.Simulated);
            orderDetails.ID = "01";
            orderDetails.Type = OrderType.Market;
            orderDetails.Quantity = 10;
            orderDetails.Security = new Security() {Symbol = "AAPL"};
            orderDetails.Side = OrderSide.BUY;
            orderDetails.Provider = TradeHubConstants.OrderExecutionProvider.Simulated;

            // Create Order Request
            OrderRequest orderRequest = new OrderRequest(orderDetails, OrderRequestType.New);

            // Add Order Details object to Provider's Order Map
            provider.AddOrder(orderDetails);

            // Rasie event to request order
            EventSystem.Publish<OrderRequest>(orderRequest);

            Thread.Sleep(5000);

            Assert.IsTrue(orderDetails.Status.Equals(OrderStatus.SUBMITTED));
        }

        [Test]
        [Category("Console")]
        public void RequestNewLimitOrder_SendRequestToServer_ReceiveOrderAcceptance()
        {
            Thread.Sleep(5000);
            OrderExecutionProvider provider = new OrderExecutionProvider(Dispatcher.CurrentDispatcher);
            provider.ProviderType = ProviderType.OrderExecution;
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = TradeHubConstants.OrderExecutionProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<OrderExecutionProvider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));

            OrderDetails orderDetails = new OrderDetails(TradeHubConstants.OrderExecutionProvider.Simulated);
            orderDetails.ID = "01";
            orderDetails.Type = OrderType.Limit;
            orderDetails.Price = 44.56M;
            orderDetails.Quantity = 10;
            orderDetails.Security = new Security() { Symbol = "AAPL" };
            orderDetails.Side = OrderSide.BUY;
            orderDetails.Provider = TradeHubConstants.OrderExecutionProvider.Simulated;

            // Create Order Request
            OrderRequest orderRequest = new OrderRequest(orderDetails, OrderRequestType.New);

            // Add Order Details object to Provider's Order Map
            provider.OrdersCollection.Add(orderDetails);

            // Rasie event to request order
            EventSystem.Publish<OrderRequest>(orderRequest);

            Thread.Sleep(5000);

            Assert.IsTrue(orderDetails.Status.Equals(OrderStatus.SUBMITTED));
        }

        [Test]
        [Category("Console")]
        public void RequestNewMarketOrderForExecution_SendRequestToServer_ReceiveOrderExecution()
        {
            Thread.Sleep(5000);
            OrderExecutionProvider provider = new OrderExecutionProvider(Dispatcher.CurrentDispatcher);
            provider.ProviderType = ProviderType.OrderExecution;
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = TradeHubConstants.OrderExecutionProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<OrderExecutionProvider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));

            OrderDetails orderDetails = new OrderDetails(TradeHubConstants.OrderExecutionProvider.Simulated);
            orderDetails.ID = "01";
            orderDetails.Type = OrderType.Market;
            orderDetails.Quantity = 10;
            orderDetails.Security = new Security() { Symbol = "AAPL" };
            orderDetails.Side = OrderSide.BUY;
            orderDetails.Provider = TradeHubConstants.OrderExecutionProvider.Simulated;

            // Create Order Request
            OrderRequest orderRequest = new OrderRequest(orderDetails, OrderRequestType.New);

            // Add Order Details object to Provider's Order Map
            provider.AddOrder(orderDetails);

            // Rasie event to request order
            EventSystem.Publish<OrderRequest>(orderRequest);

            Thread.Sleep(20000);

            Assert.IsTrue(orderDetails.Status.Equals(OrderStatus.EXECUTED));
            Assert.IsTrue(orderDetails.FillDetails.Count.Equals(1));
            //Assert.IsTrue(provider.PositionStatisticsCollection["AAPL"].Position.Equals(10));
            //Assert.IsTrue(provider.PositionStatisticsCollection["AAPL"].Pnl.Equals(-10*1.7M));
        }

        [Test]
        [Category("Console")]
        public void RequestNewLimitOrderForExecution_SendRequestToServer_ReceiveOrderExecution()
        {
            Thread.Sleep(5000);
            OrderExecutionProvider provider = new OrderExecutionProvider(Dispatcher.CurrentDispatcher);
            provider.ProviderType = ProviderType.OrderExecution;
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = TradeHubConstants.OrderExecutionProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<OrderExecutionProvider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));

            OrderDetails orderDetails = new OrderDetails(TradeHubConstants.OrderExecutionProvider.Simulated);
            orderDetails.ID = "01";
            orderDetails.Type = OrderType.Limit;
            orderDetails.Price = 44.76M;
            orderDetails.Quantity = 10;
            orderDetails.Security = new Security() { Symbol = "AAPL" };
            orderDetails.Side = OrderSide.BUY;
            orderDetails.Provider = TradeHubConstants.OrderExecutionProvider.Simulated;

            // Create Order Request
            OrderRequest orderRequest = new OrderRequest(orderDetails, OrderRequestType.New);

            // Add Order Details object to Provider's Order Map
            provider.OrdersCollection.Add(orderDetails);

            // Rasie event to request order
            EventSystem.Publish<OrderRequest>(orderRequest);

            Thread.Sleep(9000);

            Assert.IsTrue(orderDetails.Status.Equals(OrderStatus.EXECUTED));
            Assert.IsTrue(orderDetails.FillDetails.Count.Equals(1));
        }

        [Test]
        [Category("Console")]
        public void RequestOrderCancellation_SendNewOrderRequest_ReceiveOrderAcceptance_SendOrderCancellation_ReceiveOrderCancellation()
        {
            Thread.Sleep(5000);
            OrderExecutionProvider provider = new OrderExecutionProvider(Dispatcher.CurrentDispatcher);
            provider.ProviderType = ProviderType.OrderExecution;
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = TradeHubConstants.OrderExecutionProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<OrderExecutionProvider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));

            OrderDetails orderDetails = new OrderDetails(TradeHubConstants.OrderExecutionProvider.Simulated);
            orderDetails.ID = "01";
            orderDetails.Type = OrderType.Limit;
            orderDetails.Quantity = 10;
            orderDetails.Price = 45.98M;
            orderDetails.Security = new Security() { Symbol = "AAPL" };
            orderDetails.Side = OrderSide.BUY;
            orderDetails.Provider = TradeHubConstants.OrderExecutionProvider.Simulated;

            {
                // Create Order Request
                OrderRequest orderRequest = new OrderRequest(orderDetails, OrderRequestType.New);

                // Add Order Details object to Provider's Order Map
                provider.OrdersCollection.Add(orderDetails);

                // Rasie event to request order
                EventSystem.Publish<OrderRequest>(orderRequest);
            }

            Thread.Sleep(5000);

            Assert.IsTrue(orderDetails.Status.Equals(OrderStatus.SUBMITTED));
            
            {
                // Create Cancel Order Request
                OrderRequest cancelOrderRequest = new OrderRequest(orderDetails, OrderRequestType.Cancel);

                // Rasie event to request order
                EventSystem.Publish<OrderRequest>(cancelOrderRequest);
            }

            Thread.Sleep(5000);

            Assert.IsTrue(orderDetails.Status.Equals(OrderStatus.CANCELLED));
        }
    }
}
